using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;
using FinanceAccounting.BusinessLogic.Categories.Commands.CreateCategories;
using FinanceAccounting.BusinessLogic.Categories.Commands.DeleteCategories;
using FinanceAccounting.BusinessLogic.Categories.Commands.UpdateCategories;
using FinanceAccounting.BusinessLogic.Categories.Queries.GetCategoryById;
using FinanceAccounting.BusinessLogic.Categories.Queries.GetListOfCategories;
using FinanceAccounting.BusinessLogic.Common.DataTransferObjects.CategoryDto;
using FinanceAccounting.Domain.Entities;
using FinanceAccounting.WebApi.Controllers.Base;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;

namespace FinanceAccounting.WebApi.Controllers
{
    [ApiVersion("1.0")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class CategoriesController : BaseCrudController
    {
        public CategoriesController(IMediator mediator) : base(mediator)
        {
        }

        /// <summary>
        /// Gets the list of all income or expense categories
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///     GET /api/v1/categories/0
        /// </remarks>
        /// <param name="operationType">Type of category (0 - Income, 1 - Expense)</param>
        /// <param name="cancellationToken">Optional cancellation token</param>
        /// <returns>The list of user categories</returns>
        /// <response code="200">Success</response>
        /// <response code="400">If the operation type is invalid</response>
        /// <response code="401">If the user is unauthorized</response>
        [HttpGet("{operationType}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetCategories(
            OperationType operationType, CancellationToken cancellationToken = default)
        {
            var query = new GetCategoriesQuery(UserId, operationType);
            var viewModel = await Mediator.Send(query, cancellationToken);
            return Ok(viewModel);
        }

        /// <summary>
        /// Gets the required category by Id
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///     GET /api/v1/categories/find-by-id/5
        /// </remarks>
        /// <param name="categoryId">Category Id</param>
        /// <param name="cancellationToken">Optional cancellation token</param>
        /// <returns>Category with the specified id</returns>
        /// <response code="200">Success</response>
        /// <response code="400">If the category with the specified id is not found</response>
        /// <response code="401">If the user is unauthorized</response>
        [HttpGet("find-by-id/{categoryId:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetCategoryById(
            int categoryId, CancellationToken cancellationToken = default)
        {
            var query = new GetCategoryByIdQuery(UserId, categoryId);
            var viewModel = await Mediator.Send(query, cancellationToken);
            return Ok(viewModel);
        }

        /// <summary>
        /// Adds new categories to the user
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///     POST /api/v1/categories
        ///     [
        ///         {
        ///             "type": 0,
        ///             "name": "Salary"
        ///         },
        ///         {
        ///             "type": 1,
        ///             "name": "Utilities"
        ///         }
        ///     ]
        /// </remarks>
        /// <param name="categories">Categories to add</param>
        /// <param name="cancellationToken">Optional cancellation token</param>
        /// <returns>A newly created categories</returns>
        /// <response code="201">Success</response>
        /// <response code="400">If the categories to be added are invalid.</response>
        /// <response code="401">If the user is unauthorized</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> CreateCategories(
            CreateCategoryDto[] categories, CancellationToken cancellationToken = default)
        {
            var command = new CreateCategoriesCommand(UserId, categories);
            var addedCategories = await Mediator.Send(command, cancellationToken);
            return StatusCode(201, addedCategories);
        }

        /// <summary>
        /// Updates user categories with the specified ids
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///     PUT /api/v1/categories
        ///     [
        ///         {
        ///             "id": 1,
        ///             "name": "Part-time"
        ///         },
        ///         {
        ///             "id": 3,
        ///             "name": "Transport"
        ///         }
        ///     ]
        /// </remarks>
        /// <param name="categories">Categories to update</param>
        /// <param name="cancellationToken">Optional cancellation token</param>
        /// <returns>Updated categories</returns>
        /// <response code="202">Success</response>
        /// <response code="400">If the categories to be updated are invalid.</response>
        /// <response code="401">If the user is unauthorized</response>
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> UpdateCategories(
            UpdateCategoryDto[] categories, CancellationToken cancellationToken = default)
        {
            var command = new UpdateCategoriesCommand(UserId, categories);
            var updatedCategories = await Mediator.Send(command, cancellationToken);
            return StatusCode(202, updatedCategories);
        }

        /// <summary>
        /// Deletes user categories with the specified ids
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///     DELETE /api/v1/categories
        ///     [
        ///         2, 4, 8
        ///     ]
        /// </remarks>
        /// <param name="categoryIds">Category ids to delete</param>
        /// <param name="cancellationToken">Optional cancellation token</param>
        /// <returns>Deleted category ids</returns>
        /// <response code="202">Success</response>
        /// <response code="400">If the category ids to be deleted are invalid.</response>
        /// <response code="401">If the user is unauthorized</response>
        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> DeleteCategories(
            int[] categoryIds, CancellationToken cancellationToken = default)
        {
            var command = new DeleteCategoriesCommand(UserId, categoryIds);
            var deletedCategoryIds = await Mediator.Send(command, cancellationToken);
            return StatusCode(202, deletedCategoryIds);
        }
    }
}