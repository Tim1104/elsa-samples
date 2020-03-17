using Elsa.Attributes;
using Elsa.Expressions;
using Elsa.Results;
using Elsa.Samples.UserRegistration.Web.Models;
using Elsa.Services;
using Elsa.Services.Models;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Elsa.Samples.UserRegistration.Web.Activities
{
    [ActivityDefinition(Category = "TaskOrder", Description = "Activate a Task Order", Icon = "fas  fa-plus", Outcomes = new[] { OutcomeNames.Done })]
    public class CreateTaskOrder : Activity
    {
        private readonly IMongoCollection<Models.TaskOrder> _store;
        private readonly IIdGenerator _idGenerator;
        public CreateTaskOrder(IMongoCollection<Models.TaskOrder> store, IIdGenerator idGenerator)
        {
            _store = store;
            _idGenerator = idGenerator;
        }


        [ActivityProperty(Hint = "Enter an expression that evaluates to the name of the user to create.")]
        public WorkflowExpression<string> OrderDescription
        {
            get => GetState<WorkflowExpression<string>>();
            set => SetState(value);
        }


        [ActivityProperty(Hint = "Enter an expression that evaluates to the name of the user to create.")]
        public WorkflowExpression<string> Assignee
        {
            get => GetState<WorkflowExpression<string>>();
            set => SetState(value);
        }

        protected override async Task<ActivityExecutionResult> OnExecuteAsync(WorkflowExecutionContext context, CancellationToken cancellationToken)
        {
            var description = await context.EvaluateAsync(OrderDescription, cancellationToken);
            var assignee = await context.EvaluateAsync(Assignee, cancellationToken);

            var orderEntity = new TaskOrder
            {
                Id = _idGenerator.Generate(),
                Description = description,
                Assignee = assignee
            };

            await _store.InsertOneAsync(orderEntity, cancellationToken: cancellationToken);

            Output.SetVariable("TaskOrder", orderEntity);
            return Done();
        }

    }
}
