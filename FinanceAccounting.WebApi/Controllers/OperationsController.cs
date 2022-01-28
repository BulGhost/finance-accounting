using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading;
using System.Threading.Tasks;
using FinanceAccounting.BusinessLogic.Common.DataTransferObjects.OperationDto;
using FinanceAccounting.BusinessLogic.Operations.Commands.AddOperations;
using FinanceAccounting.BusinessLogic.Operations.Commands.DeleteOperations;
using FinanceAccounting.BusinessLogic.Operations.Commands.UpdateOperations;
using FinanceAccounting.BusinessLogic.Operations.Queries.GetDaysOperationsReport;
using FinanceAccounting.BusinessLogic.Operations.Queries.GetPeriodsOperationsReport;
using FinanceAccounting.WebApi.Controllers.Base;
using FinanceAccounting.WebApi.ViewModels;
using FinanceAccounting.WebApi.ViewModels.HelperClasses;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;

namespace FinanceAccounting.WebApi.Controllers
{
    [ApiVersion("1.0")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class OperationsController : BaseCrudController
    {
        public OperationsController(IMediator mediator) : base(mediator)
        {
        }

        /// <summary>
        /// Generates the operations report for the specified date
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///     GET /api/v1/operations/days-report?date=2022-01-10
        /// </remarks>
        /// <param name="date">Date on which the report is to be generated</param>
        /// <param name="cancellationToken">Optional cancellation token</param>
        /// <returns>Total income and expense sum and list of operations for the specified date</returns>
        /// <response code="200">Success</response>
        /// <response code="400">If the specified date is invalid</response>
        /// <response code="401">If the user is unauthorized</response>
        [HttpGet("days-report")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetDaysOperationsReport(
            [FromQuery] DateTime date, CancellationToken cancellationToken = default)
        {
            var query = new GetDaysOperationsQuery(UserId, date);
            var operations = await Mediator.Send(query, cancellationToken);
            OperationsReport report = new ReportBuilder().BuildOperationsReport(operations);
            return Ok(report);
        }

        /// <summary>
        /// Generates the operations report for the specified period
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///     GET api/v1/Operations/period-report?startDate=2020-10-10&amp;finalDate=2020-10-11
        /// </remarks>
        /// <param name="startDate">Start date on which the report is to be generated</param>
        /// <param name="finalDate">Final on which the report is to be generated</param>
        /// <param name="cancellationToken">Optional cancellation token</param>
        /// <returns>Total income and expense sum and list of operations for the specified period</returns>
        /// <response code="200">Success</response>
        /// <response code="400">If one or both of the specified dates are invalid</response>
        /// <response code="401">If the user is unauthorized</response>
        [HttpGet("period-report")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetPeriodsOperationsReport(
            [FromQuery] DateTime startDate, [FromQuery] DateTime finalDate, CancellationToken cancellationToken = default)
        {
            var query = new GetPeriodsOperationsQuery(UserId, startDate, finalDate);
            var operations = await Mediator.Send(query, cancellationToken);
            OperationsReport report = new ReportBuilder().BuildOperationsReport(operations);
            return Ok(report);
        }

        /// <summary>
        /// Adds new operations to the user
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///     POST /api/v1/operations
        ///     [
        ///         {
        ///             "date": "2022-01-10",
        ///             "categoryId": 27,
        ///             "sum": 1500,
        ///             "details": "Details"
        ///         },
        ///         {
        ///             "date": "2022-01-11",
        ///             "categoryId": 38,
        ///             "sum": 750
        ///         },
        ///     ]
        /// </remarks>
        /// <param name="operations">Operations to add</param>
        /// <param name="cancellationToken">Optional cancellation token</param>
        /// <returns>A newly created operations</returns>
        /// <response code="201">Success</response>
        /// <response code="400">If the operations to be added are invalid.</response>
        /// <response code="401">If the user is unauthorized</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> AddOperations(
            CreateOperationDto[] operations, CancellationToken cancellationToken = default)
        {
            var command = new AddOperationsCommand(UserId, operations);
            var addedOperations = await Mediator.Send(command, cancellationToken);
            return StatusCode(201, addedOperations);
        }

        /// <summary>
        /// Updates user operations with the specified ids
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///     PUT /api/v1/operations
        ///     [
        ///         {
        ///             "id": 42,
        ///             "date": "2022-01-08",
        ///             "categoryId": 12,
        ///             "sum": 1200,
        ///             "details": "Details"
        ///         },
        ///         {
        ///             "id": 61,
        ///             "date": "2022-01-12",
        ///             "categoryId": 8,
        ///             "sum": 600
        ///         }
        ///     ]
        /// </remarks>
        /// <param name="operations">Operations to update</param>
        /// <param name="cancellationToken">Optional cancellation token</param>
        /// <returns>Updated operations</returns>
        /// <response code="202">Success</response>
        /// <response code="400">If the operations to be updated are invalid.</response>
        /// <response code="401">If the user is unauthorized</response>
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> UpdateOperations(
            UpdateOperationDto[] operations, CancellationToken cancellationToken = default)
        {
            var command = new UpdateOperationsCommand(UserId, operations);
            var updatedOperations = await Mediator.Send(command, cancellationToken);
            return StatusCode(202, updatedOperations);
        }

        /// <summary>
        /// Deletes user operations with the specified ids
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///     DELETE /api/v1/operations
        ///     [
        ///         64, 73, 89
        ///     ]
        /// </remarks>
        /// <param name="operationIds">Operation ids to delete</param>
        /// <param name="cancellationToken">Optional cancellation token</param>
        /// <returns>Deleted operation ids</returns>
        /// <response code="202">Success</response>
        /// <response code="400">If the operation ids to be deleted are invalid.</response>
        /// <response code="401">If the user is unauthorized</response>
        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> DeleteOperations(
            int[] operationIds, CancellationToken cancellationToken = default)
        {
            var command = new DeleteOperationsCommand(UserId, operationIds);
            var deletedOperationIds = await Mediator.Send(command, cancellationToken);
            return StatusCode(202, deletedOperationIds);
        }
    }
}
