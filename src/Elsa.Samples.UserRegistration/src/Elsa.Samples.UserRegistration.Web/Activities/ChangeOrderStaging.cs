using Elsa.Attributes;
using Elsa.Expressions;
using Elsa.Results;
using Elsa.Services;
using Elsa.Services.Models;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Elsa.Samples.UserRegistration.Web.Activities
{
    [ActivityDefinition(Category = "TaskOrder", Description = "Change a Task Order", Icon = "fas fa-wrench", Outcomes = new[] { OutcomeNames.Done, "Not Found" })]
    public class ChangeOrderStaging : Activity
    {
        private readonly IMongoCollection<Models.TaskOrder> _store;
        public ChangeOrderStaging(IMongoCollection<Models.TaskOrder> store)
        {
            _store = store;
        }

        [ActivityProperty(Hint = "Enter an expression that evaluates to the ID of the order to activate.")]
        public WorkflowExpression<string> OrderId
        {
            get => GetState<WorkflowExpression<string>>();
            set => SetState(value);
        }

        [ActivityProperty(Hint = "Enter an expression that evaluates to the Staging of the order to activate.")]
        public WorkflowExpression<string> Staging
        {
            get => GetState<WorkflowExpression<string>>();
            set => SetState(value);
        }

        protected override async Task<ActivityExecutionResult> OnExecuteAsync(WorkflowExecutionContext context, CancellationToken cancellationToken)
        {
            var orderId = await context.EvaluateAsync(OrderId, cancellationToken);
            var orderEntity = await _store.AsQueryable().FirstOrDefaultAsync(x => x.Id == orderId, cancellationToken);

            if (orderEntity == null)
                return Outcome("Not Found");

            var staging = await context.EvaluateAsync(Staging, cancellationToken);

            orderEntity.OrderStaging=  (Models.TaskOrderStaging)Enum.Parse(typeof(Models.TaskOrderStaging), staging);
            await _store.ReplaceOneAsync(x => x.Id == orderId, orderEntity, cancellationToken: cancellationToken);

            return Done();
        }
    }
}
