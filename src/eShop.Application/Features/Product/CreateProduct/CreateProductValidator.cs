﻿using eShop.Domain.ValueObjects;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eShop.Application.Features.Product.CreateProduct
{
    public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
    {
        public CreateProductCommandValidator()
        {
            RuleFor(e => e.VendorCode)
                .NotEmpty()
                .MinimumLength(4)
                .MaximumLength(128);

            RuleFor(e => e.Name)
                .NotEmpty()
                .MinimumLength(1)
                .MaximumLength(256);

            RuleFor(e => e.Description)
                .NotEmpty()
                .MinimumLength(12)
                .MaximumLength(4096);

            RuleFor(e => e.Price)
                .GreaterThanOrEqualTo(0)
                .LessThanOrEqualTo(Money.MaxPrice);

            RuleFor(e => e.Currency)
                .NotEmpty();
        }
    }
}
