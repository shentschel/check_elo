using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using check_elo.Configuration;
using check_elo.Parameters;
using check_elo.Response;
using EloixClient.IndexServer;
using pcm.IXClient7;

namespace check_elo.Commands.impl
{
    public class WorkflowNodesCommand : EloCommand<WorkflowNodesParameters>
    {
        private readonly List<WorkflowErrorNode> _errorNodes = new List<WorkflowErrorNode>();

        public WorkflowNodesCommand(IXClient client, Settings settings, WorkflowNodesParameters parameters, CheckResult checkResult) : base(
            client, settings, parameters, checkResult)
        {
        }

        public override bool Run()
        {
            Log.Debug($"Running command '{GetType()}'");
            
            return ExecuteEloCommand(ReadWorkflowNodes);
        }

        private void ReadWorkflowNodes() {
            var findTasksInfo = new FindTasksInfo
                {
                    userIds = Parameters.WorkflowUsers.Split(','),
                    inclWorkflows = true
                };

                var userTasks = Client.IX.findFirstTasks(findTasksInfo, 500).tasks;

                CheckResult.Message = $"No task found for users '{Parameters.WorkflowUsers}', | nodes=0; errorNodes=0";
                CheckResult.ExitCode = ExitCode.Ok;

                if (userTasks.Length <= 0) return;

                foreach (var task in userTasks)
                {
                    var taskActivationDate = DateTime.ParseExact(task.wfNode.activateDateIso, "yyyyMMddHHmmss",
                        CultureInfo.InvariantCulture);
                    var differenceTimespan = DateTime.UtcNow - taskActivationDate;
                    var minutesSinceActivation = Convert.ToInt32(differenceTimespan.TotalMinutes);

                    if (minutesSinceActivation <= Parameters.WarningMinutes) continue;

                    var node = new WorkflowErrorNode(task.wfNode.nodeName, task.wfNode.userName,
                        minutesSinceActivation)
                    {
                        ExitCode = minutesSinceActivation > Parameters.CriticalMinutes
                            ? ExitCode.Critical
                            : ExitCode.Warning
                    };
                    _errorNodes.Add(node);
                }

                if (_errorNodes.Count > 0)
                {
                    var criticalNodesCount = _errorNodes.Count(node => node.ExitCode == ExitCode.Critical);
                    CheckResult.ExitCode = criticalNodesCount > 0 ? ExitCode.Critical : ExitCode.Warning;

                    foreach (var errorNode in _errorNodes)
                        CheckResult.Message +=
                            $"User: {errorNode.EloUser} - Node: {errorNode.Name} - Active since: {errorNode.MinutesSinceActivation} Minutes | ";

                    CheckResult.Message += $"nodes={userTasks.Length}; errorNodes={_errorNodes.Count}";
                }
                else
                {
                    CheckResult.Message =
                        $"Found '{userTasks.Length}' nodes for users '{Parameters.WorkflowUsers}'. No task is overdue.";
                }
        }
    }
}