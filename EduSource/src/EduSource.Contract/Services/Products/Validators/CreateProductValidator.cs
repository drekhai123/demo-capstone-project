using EduSource.Contract.Enumarations.Product;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using System.Linq;

namespace EduSource.Contract.Services.Products.Validators;

public class CreateProductValidator : AbstractValidator<Command.CreateProductCommand>
{
    private readonly List<string> _allowedImageExtensions = new() { ".jpg", ".jpeg", ".png", ".gif", ".bmp" };
    private readonly List<string> _allowedFileExtensions = new() { ".pdf", ".ppt", ".pptx", ".zip", ".zar" };
    public CreateProductValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Product name is required.");

        RuleFor(x => x.Price)
            .GreaterThan(0).WithMessage("Price must be greater than zero.");

        RuleFor(x => x.Category)
            .NotNull().WithMessage("Category is required.");

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Description is required.");

        RuleFor(x => x.ContentType)
            .NotNull().WithMessage("Content Type is required.");

        RuleFor(x => x.Unit)
            .GreaterThan(0).WithMessage("Unit must be greater than zero.")
            .When(x => x.ContentType == ContentType.Unit);
        RuleFor(x => x.UploadType)
            .NotNull().WithMessage("Upload Type is required.");

        RuleFor(x => x.TotalPage)
            .GreaterThan(0).WithMessage("Total Page must be greater than zero.");

        RuleFor(x => x.Size)
            .GreaterThan(0).WithMessage("Size must be greater than zero.");

        RuleFor(x => x.MainImage)
            .NotNull().WithMessage("Main Image is required.")
            .Must(BeAValidImage).WithMessage("Main Image must be in image format (JPG, JPEG, PNG, GIF, BMP).");

        RuleFor(x => x.OtherImages)
            .NotNull().WithMessage("Other Images are required.")
            .Must(images => images != null && images.All(BeAValidImage))
            .WithMessage("All Other Images must be in image format (JPG, JPEG, PNG, GIF, BMP).");

        RuleFor(x => x.File)
            .NotNull().WithMessage("File is required.")
            .Must(BeAValidFile).WithMessage("File must be in PDF, PP, ZIP, or ZAR format.");

        RuleFor(x => x.FileDemo)
            .NotNull().WithMessage("FileDemo is required.")
            .Must(BeAValidFile).WithMessage("FileDemo must be in PDF, PP, ZIP, or ZAR format.");

        RuleFor(x => x.BookId)
            .NotEmpty().WithMessage("Book ID is required.");

        RuleFor(x => x.AccountId)
            .NotEmpty().WithMessage("Account ID is required.");
    }

    private bool BeAValidImage(IFormFile file)
    {
        if (file == null) return false;
        var extension = System.IO.Path.GetExtension(file.FileName).ToLower();
        return _allowedImageExtensions.Contains(extension);
    }

    private bool BeAValidFile(IFormFile file)
    {
        if (file == null) return false;
        var extension = System.IO.Path.GetExtension(file.FileName).ToLower();
        return _allowedFileExtensions.Contains(extension);
    }
}
