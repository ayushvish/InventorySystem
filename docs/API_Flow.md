Controllers (files, route base, actions)

- AuthController (src/Inventory.API/Controllers/AuthController.cs)
  - Route: /api/v1/Auth
  - Attributes: [ApiController], [Route("api/v1/[controller]")]
  - Actions:
	- POST /api/v1/Auth/login — Login(LoginRequest). [AllowAnonymous]

- ApiControllerBase (src/Inventory.API/Controllers/ApiControllerBase.cs)
  - Base route/template used by other controllers: /api/v{version:apiVersion}/[controller]
  - Provides protected Mediator property (lazy-resolve IMediator)

- CategoriesController (src/Inventory.API/Controllers/CategoriesController.cs)
  - Route (inherited): /api/v1/Categories
  - Attributes: [Authorize]
  - Actions:
	- POST /api/v1/Categories — Create(CreateCategoryRequest)
	- GET /api/v1/Categories — GetPaginated(QueryParameters)
	- GET /api/v1/Categories/{id:int} — GetById(int id)
	- PUT /api/v1/Categories/{id:int} — Update(int id, UpdateCategoryRequest)
	- DELETE /api/v1/Categories/{id:int} — Delete(int id)

- ManufacturersController (src/Inventory.API/Controllers/ManufacturersController.cs)
  - Route: /api/v1/Manufacturers (inherited)
  - [Authorize]
  - Actions: POST, GET (paginated), GET/{id}, PUT/{id}, DELETE/{id}

- MedicinesController (src/Inventory.API/Controllers/MedicinesController.cs)
  - Route: /api/v1/Medicines
  - [Authorize]
  - Actions: POST, GET (paginated), GET/{id}, PUT/{id}, DELETE/{id}

- SuppliersController (src/Inventory.API/Controllers/SuppliersController.cs)
  - Route: /api/v1/Suppliers
  - [Authorize]
  - Actions: POST, GET (paginated), GET/{id}, PUT/{id}, DELETE/{id}

- UnitsController (src/Inventory.API/Controllers/UnitsController.cs)
  - Route: /api/v1/Units
  - [Authorize]
  - Actions: POST, GET (paginated), GET/{id}, PUT/{id}, DELETE/{id}

- WarehouseLocationsController (src/Inventory.API/Controllers/WarehouseLocationsController.cs)
  - Route: /api/v1/WarehouseLocations
  - [Authorize]
  - Actions: POST, GET (paginated), GET/{id}, PUT/{id}, DELETE/{id}

- WarehousesController (src/Inventory.API/Controllers/WarehousesController.cs)
  - Route: /api/v1/Warehouses
  - [Authorize]
  - Actions: POST, GET (paginated), GET/{id}, PUT/{id}, DELETE/{id}

Example end-to-end trace: Create Category (full flow)
1. HTTP request
   - Client POST /api/v1/Categories with JSON body matching CreateCategoryRequest.

2. Controller
   - CategoriesController.Create([FromBody] CreateCategoryRequest request)
   - Calls Mediator.Send(new CreateCategoryCommand(request)).
   - File: src/Inventory.API/Controllers/CategoriesController.cs

3. MediatR dispatch
   - IRequest<CreateCategoryCommand, CategoryResponse> routed to CreateCategoryCommandHandler.
   - Handler file: src/Inventory.Application/Features/Category/Commands/CategoryCommands.cs

4. Command handler logic
   - Gets repository: _unitOfWork.Repository<Domain.Entities.Category>().
   - Checks existence: repository.ExistsAsync(c => c.CategoryName == request.CategoryName).
	 - If exists -> throws ValidationException.
   - Maps DTO to entity: _mapper.Map<Domain.Entities.Category>(request).
	 - Mapping profile: src/Inventory.Application/Features/Category/Mappings/CategoryMappingProfile.cs
	 - DTOs: src/Inventory.Application/Features/Category/DTOs/CategoryDtos.cs
   - Calls repository.AddAsync(entity).
	 - Generic repository: src/Inventory.Infrastructure/Data/Repositories/GenericRepository.cs
   - Calls _unitOfWork.SaveChangesAsync(cancellationToken).
	 - UnitOfWork: src/Inventory.Infrastructure/Data/Repositories/UnitOfWork.cs

5. DbContext persistence & auditing
   - InventoryDbContext.SaveChangesAsync overrides:
	 - Sets CreatedAt/CreatedBy and IsDeleted=false for added entities.
	 - Converts Deleted state into soft-delete (IsDeleted=true).
	 - File: src/Inventory.Infrastructure/Data/InventoryDbContext.cs
   - Changes flushed to database (EF Core migrations present in src/Inventory.Infrastructure/Migrations).

6. Response mapping
   - Handler returns mapped CategoryResponse (AutoMapper).
   - Controller wraps in ApiResponse<CategoryResponse> and returns Ok(...).
   - Client receives 200 OK with created category DTO.

Key files to inspect
- Controllers: src/Inventory.API/Controllers/*.cs
- Commands/Queries/DTOs: src/Inventory.Application/Features/*
- Repositories/UnitOfWork: src/Inventory.Infrastructure/Data/Repositories/*
- DbContext & configurations: src/Inventory.Infrastructure/Data/*
