using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Text;

namespace Yaban.Web.TagHelpers;

[HtmlTargetElement("datatable")]
public class DatatableTagHelper : TagHelper
{
    [HtmlAttributeName("ajax-url")]
    public string AjaxUrl { get; set; }

    // Kolonları belirttiğimiz string. Örn: "id, name, price"
    [HtmlAttributeName("columns")]
    public string Columns { get; set; }

    public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
    {
        output.TagName = "table";
        output.Attributes.SetAttribute("class", "table table-striped table-bordered display responsive nowrap datatable-ajax");
        output.Attributes.SetAttribute("style", "width:100%");
        output.Attributes.SetAttribute("data-ajax-url", AjaxUrl);

        // Eğer 'columns' attribute'ü doluysa, thead'i buradan oluştur.
        if (!string.IsNullOrEmpty(Columns))
        {
            var acontent = new StringBuilder();
            acontent.Append("<thead><tr>");

            // "id, name, price" string'ini virgüllerden ayır.
            var columnNames = Columns.Split(',');

            foreach (var name in columnNames)
            {
                var trimmedName = name.Trim();
                if (string.IsNullOrWhiteSpace(trimmedName)) continue;

                // Hem 'data-name' attribute'ü hem de görünen başlık için aynı ismi kullan.
                // Bu isim, JSON'daki property ismiyle eşleşmelidir.
                acontent.Append($"<th data-name=\"{trimmedName}\">{trimmedName}</th>");
            }

            acontent.Append("</tr></thead>");
            acontent.Append("<tbody></tbody>");

            output.Content.SetHtmlContent(acontent.ToString());
        }
        else
        {
            // Eğer 'columns' attribute'ü boşsa, etiketler arasındaki 
            // manuel olarak yazılmış içeriği kullan.
            var childContent = await output.GetChildContentAsync();
            var acontent = new StringBuilder();
            acontent.Append(childContent.GetContent());
            acontent.Append("<tbody></tbody>");
            output.Content.SetHtmlContent(acontent.ToString());
        }
    }
}