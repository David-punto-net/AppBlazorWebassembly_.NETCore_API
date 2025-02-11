using Microsoft.EntityFrameworkCore;
using Orders.Backend.Data;
using Orders.Backend.Helpers;
using Orders.Backend.Repositories.Interfaces;
using Orders.Shared.DTOs;
using Orders.Shared.Entities;
using Orders.Shared.Response;

namespace Orders.Backend.Repositories.Implementations
{
    public class ProductsRepository : GenericRepository<Product>, IProductsRepository
    {
        private readonly DataContext _context;
        private readonly IFileStorage _fileStorage;

        public ProductsRepository(DataContext context, IFileStorage fileStorage) : base(context)
        {
            _context = context;
            _fileStorage = fileStorage;
        }

        public override async Task<ActionResponse<Product>> DeleteAsync(int Id)
        {
            var product = await _context.Products
                                 .Include(x => x.ProductCategories)
                                 .Include(x => x.ProductImages)
                                 .FirstOrDefaultAsync(x => x.Id == Id);
            if (product == null)
            {
                return new ActionResponse<Product>
                {
                    WassSuccees = false,
                    Message = "Producto no encontrado"
                };
            }

            foreach (var productImage in product.ProductImages!)
            {
                await _fileStorage.RemoveFileAsync(productImage.Image, "products");
            }

            try
            {
                _context.ProductCategories.RemoveRange(product.ProductCategories!);
                _context.ProductImages.RemoveRange(product.ProductImages!);
                _context.Products.Remove(product);
                await _context.SaveChangesAsync();

                return new ActionResponse<Product>
                {
                    WassSuccees = true,
                };
            }
            catch
            {
                return new ActionResponse<Product>
                {
                    WassSuccees = false,
                    Message = "No se puede borrar el producto, porque tiene registros relacionados"
                };
            }
        }

        public override async Task<ActionResponse<IEnumerable<Product>>> GetAsync(PaginationDTO pagination)
        {
            var queryable = _context.Products
                            .Include(x => x.ProductImages)
                            .Include(x => x.ProductCategories!)
                            .ThenInclude(x => x.Category)
                            .AsQueryable();

            if (!string.IsNullOrEmpty(pagination.Filter))
            {
                var filter = pagination.Filter.ToLower();
                queryable = queryable.Where(x => x.Name.ToLower().Contains(filter));
            }

            return new ActionResponse<IEnumerable<Product>>
            {
                WassSuccees = true,
                Result = await queryable.Skip(pagination.Page).Take(pagination.RecordsNumber).ToListAsync()
            };
        }

        public override async Task<ActionResponse<int>> GetTotalRecordAsync(PaginationDTO pagination)
        {
            var queryable = _context.Products.AsQueryable();

            if (!string.IsNullOrEmpty(pagination.Filter))
            {
                var filter = pagination.Filter.ToLower();
                queryable = queryable.Where(x => x.Name.ToLower().Contains(filter));
            }

            int count = await queryable.CountAsync();
            return new ActionResponse<int>
            {
                WassSuccees = true,
                Result = count
            };
        }

        public override async Task<ActionResponse<Product>> GetAsync(int id)
        {
            var product = await _context.Products
                                .Include(x => x.ProductImages)
                                .Include(x => x.ProductCategories!)
                                .ThenInclude(x => x.Category)
                                .FirstOrDefaultAsync(x => x.Id == id);
            if (product == null)
            {
                return new ActionResponse<Product>
                {
                    WassSuccees = false,
                    Message = "Producto no existe"
                };
            }
            return new ActionResponse<Product>
            {
                WassSuccees = true,
                Result = product
            };
        }

        public async Task<ActionResponse<Product>> AddFullAsync(ProductDTO productDTO)
        {
            try
            {
                var newProduct = new Product
                {
                    Name = productDTO.Name,
                    Description = productDTO.Description,
                    Price = productDTO.Price,
                    Stock = productDTO.Stock,
                    ProductCategories = new List<ProductCategory>(),
                    ProductImages = new List<ProductImage>()
                };

                foreach (var productImage in productDTO.ProductImages!)
                {
                    var photoProduct = Convert.FromBase64String(productImage);
                    newProduct.ProductImages.Add(new ProductImage
                    {
                        //TODO: Almacena Imagen
                        Image = await _fileStorage.SaveFileAsync(photoProduct, ".jpg", "products")
                    });
                }

                foreach (var productCategoryId in productDTO.ProductCategoryIds!)
                {
                    var category = await _context.Categories.FirstOrDefaultAsync(x => x.Id == productCategoryId);
                    if (category != null)
                    {
                        newProduct.ProductCategories.Add(new ProductCategory { Category = category });
                    }
                }

                _context.Add(newProduct);
                await _context.SaveChangesAsync();

                return new ActionResponse<Product>
                {
                    WassSuccees = true,
                    Result = newProduct
                };
            }
            catch (DbUpdateException)
            {
                return new ActionResponse<Product>
                {
                    WassSuccees = false,
                    Message = "Ya existe un producto con el mismo nombre."
                };
            }
            catch (Exception exception)
            {
                return new ActionResponse<Product>
                {
                    WassSuccees = false,

                    Message = exception.Message
                };
            }
        }

        public async Task<ActionResponse<Product>> UpdateFullAsync(ProductDTO productDTO)
        {
            try
            {
                var product = await _context.Products
                .Include(x => x.ProductCategories!)
                .ThenInclude(x => x.Category)
                .FirstOrDefaultAsync(x => x.Id == productDTO.Id);
                if (product == null)
                {
                    return new ActionResponse<Product>
                    {
                        WassSuccees = false,
                        Message = "Producto no existe"
                    };
                }

                product.Name = productDTO.Name;
                product.Description = productDTO.Description;
                product.Price = productDTO.Price;
                product.Stock = productDTO.Stock;
                _context.ProductCategories.RemoveRange(product.ProductCategories!);
                product.ProductCategories = new List<ProductCategory>();

                foreach (var productCategoryId in productDTO.ProductCategoryIds!)
                {
                    var category = await _context.Categories.FindAsync(productCategoryId);
                    if (category != null)
                    {
                        _context.ProductCategories.Add(new ProductCategory
                        {
                            CategoryId = category.Id,
                            ProductId = product.Id
                        });
                    }
                }

                _context.Update(product);
                await _context.SaveChangesAsync();
                return new ActionResponse<Product>
                {
                    WassSuccees = true,
                    Result = product
                };
            }
            catch (DbUpdateException)
            {
                return new ActionResponse<Product>
                {
                    WassSuccees = false,
                    Message = "Ya existe un producto con el mismo nombre."
                };
            }
            catch (Exception exception)
            {
                return new ActionResponse<Product>
                {
                    WassSuccees = false,
                    Message = exception.Message
                };
            }
        }

        public async Task<ActionResponse<ImageDTO>> AddImageAsync(ImageDTO imageDTO)
        {
            var product = await _context.Products
            .Include(x => x.ProductImages)
            .FirstOrDefaultAsync(x => x.Id == imageDTO.ProductId);
            if (product == null)
            {
                return new ActionResponse<ImageDTO>
                {
                    WassSuccees = false,
                    Message = "Producto no existe"
                };
            }
            for (int i = 0; i < imageDTO.Images.Count; i++)
            {
                if (!imageDTO.Images[i].StartsWith("https://"))
                {
                    var photoProduct = Convert.FromBase64String(imageDTO.Images[i]);
                    imageDTO.Images[i] = await _fileStorage.SaveFileAsync(photoProduct, ".jpg", "products");
                    product.ProductImages!.Add(new ProductImage { Image = imageDTO.Images[i] });
                }
            }
            _context.Update(product);
            await _context.SaveChangesAsync();
            return new ActionResponse<ImageDTO>
            {
                WassSuccees = true,
                Result = imageDTO
            };
        }

        public async Task<ActionResponse<ImageDTO>> RemoveLastImageAsync(ImageDTO imageDTO)
        {
            var product = await _context.Products
            .Include(x => x.ProductImages)
            .FirstOrDefaultAsync(x => x.Id == imageDTO.ProductId);
            if (product == null)
            {
                return new ActionResponse<ImageDTO>
                {
                    WassSuccees = false,
                    Message = "Producto no existe"
                };
            }
            if (product.ProductImages is null || product.ProductImages.Count == 0)
            {
                return new ActionResponse<ImageDTO>
                {
                    WassSuccees = true,

                    Result = imageDTO
                };
            }
            var lastImage = product.ProductImages.LastOrDefault();
            await _fileStorage.RemoveFileAsync(lastImage!.Image, "products");
            _context.ProductImages.Remove(lastImage);
            await _context.SaveChangesAsync();
            imageDTO.Images = product.ProductImages.Select(x => x.Image).ToList();
            return new ActionResponse<ImageDTO>
            {
                WassSuccees = true,
                Result = imageDTO
            };
        }
    }
}