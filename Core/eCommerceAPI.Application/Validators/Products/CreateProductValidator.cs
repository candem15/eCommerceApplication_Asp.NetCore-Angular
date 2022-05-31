using eCommerceAPI.Application.ViewModels.Products;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerceAPI.Application.Validators.Products
{
    public class CreateProductValidator : AbstractValidator<VM_CreateProduct>
    {
        public CreateProductValidator()
        {
            RuleFor(c => c.Name)
                .NotEmpty()
                .NotNull()
                    .WithMessage("Product name can not be empty!")
                .MaximumLength(200)
                .MinimumLength(1)
                    .WithMessage("Length of product name must be between 1 and 200 characters!");

            RuleFor(c => c.Stock)
                .NotEmpty()
                .NotNull()
                    .WithMessage("Stock can not be empty!")
                .GreaterThanOrEqualTo(0)
                    .WithMessage("Stock must be greater or equal then 0!");

            RuleFor(c => c.Price)
                .NotEmpty()
                .NotNull()
                    .WithMessage("Price can not be empty!")
                .GreaterThanOrEqualTo(0)
                    .WithMessage("Price must be greater or equal then 0!");

        }
    }
}
