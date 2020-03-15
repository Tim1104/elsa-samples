using Elsa.Attributes;
using Elsa.Expressions;
using Elsa.Models;
using Elsa.Results;
using Elsa.Services;
using Elsa.Services.Models;
using Elsa.Activities.Workflows.Extensions;
 
using System.Threading;
using System.Threading.Tasks;

namespace Elsa.Samples.UserRegistration.Web.Activities
{
    [ActivityDefinition(Category = "Plan", Description = "Plan a B", Icon = "fas fa-user-check", Outcomes = new[] { OutcomeNames.Done })]
    public class ActivateB : Activity
    {
        private readonly IWorkflowInvoker _invoker;

        public ActivateB(IWorkflowInvoker invoker)
        {
            _invoker = invoker;
        }
        [ActivityProperty(Hint = "Enter an Prefix.")]
        public WorkflowExpression<string> Prefix
        {
            get => GetState<WorkflowExpression<string>>();
            set => SetState(value);
        }

        protected override async Task<ActivityExecutionResult> OnExecuteAsync(WorkflowExecutionContext context, CancellationToken cancellationToken)
        {
            var prefix = await context.EvaluateAsync(Prefix, cancellationToken);

            var input = new Variables();
            input.SetVariable("Prefix", prefix);

            await _invoker.TriggerSignalAsync("SubReadPrefix", input);

            //Output.SetVariable("Content", prefix + "B");
            return Done();
        }
    }
}
