using FluentValidation;
using LibraryAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryAPI.Validators
{
    public class BookValidator : AbstractValidator<Book>
    {
        public BookValidator() 
        {
            RuleFor(x => x.Name).NotNull().NotEmpty().WithMessage("Name Cannot Empty");
            RuleFor(x => x.Name).MaximumLength(100).WithMessage("Name Must be less than 100 Characters");
            RuleFor(x => x.Price).NotNull().NotEmpty().WithMessage("Price Cannot Empty");
            RuleFor(x=>x.CreatedDate).NotNull().NotEmpty().WithMessage("CreatedDate Cannot Empty");
        }
    }
}
