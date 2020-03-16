using Elsa.Attributes;
using Elsa.Expressions;
using Elsa.Results;
using Elsa.Services;
using Elsa.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Elsa.Samples.UserRegistration.Web.Activities
{
    [ActivityDefinition(Category = "Plan", Description = "Plan a E", Icon = "fas fa-user-check", Outcomes = new[] { OutcomeNames.Done, "Not Found" })]
    public class ActivateE : Activity
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

            Output.SetVariable("Content", prefix + "E");
            return Done();
        }

        protected override ActivityExecutionResult OnResume(WorkflowExecutionContext context)
        {
            var prefix = GetPerfix(context);
            return OnResume(context);
        }
        private string GetPerfix(WorkflowExecutionContext context) =>
          (string)context.Workflow.Input.GetVariable("Prefix");
    }
}
