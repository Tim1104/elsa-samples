using System.Threading;
using System.Threading.Tasks;
using Elsa.Attributes;
using Elsa.Expressions;
using Elsa.Extensions;
using Elsa.Results;
using Elsa.Samples.UserRegistration.Web.Models;
using Elsa.Samples.UserRegistration.Web.Services;
using Elsa.Services;
using Elsa.Services.Models;
using MongoDB.Driver;

namespace Elsa.Samples.UserRegistration.Web.Activities
{
    [ActivityDefinition(Category = "Plan", Description = "Plan a A", Icon = "fas fa-user-check", Outcomes = new[] { OutcomeNames.Done })]
    public class ActivateA : Activity
    {
        [ActivityProperty(Hint = "Enter an Prefix.")]
        public WorkflowExpression<string> Prefix
        {
            get => GetState<WorkflowExpression<string>>();
            set => SetState(value);
        }

        protected override async Task<ActivityExecutionResult> OnExecuteAsync(WorkflowExecutionContext context, CancellationToken cancellationToken)
        {
            var prefix = await context.EvaluateAsync(Prefix, cancellationToken);
             
            Output.SetVariable("Content", prefix + "A");
            return Done(); 
        }
    }
}
