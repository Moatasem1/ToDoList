using Infrastructure.Persistence.Configurations.Users;
using Microsoft.AspNetCore.Hosting;

namespace Infrastructure.Email;

public class EmailTemplateBuilder(IWebHostEnvironment webHostEnvironment)
{
    public  string BuildEmailBody(string templateName, Dictionary<string, string> replacements)
    {
        var layout = GetEmailTemplate("Layout.html");
        var template = GetEmailTemplate(templateName + ".html");


        foreach (var replacement in replacements)
            template = template.Replace("{{" + replacement.Key + "}}", replacement.Value);

        layout = layout.Replace("{{Body}}", template);

        return layout;
    }

    public  string GetEmailTemplate(string templateName)
    {
        string filePath = Path.Combine(webHostEnvironment.ContentRootPath, "Templates", "Emails", templateName);
        if (File.Exists(filePath))
        {
            return File.ReadAllText(filePath);
        }

        var assembly = typeof(UserConfigurations).Assembly;
        using var stream = assembly.GetManifestResourceStream($"Infrastructure.Email.Templates.{templateName}");
        if (stream != null)
        {
            using var reader = new StreamReader(stream);
            return reader.ReadToEnd();
        }

        throw new FileNotFoundException($"Email template '{templateName}' not found");
    }
}