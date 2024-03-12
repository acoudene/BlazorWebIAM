// Changelogs Date  | Author                | Description
// 2023-02-22       | Anthony Coudène (ACE) | Creation

namespace BlazorWebIAM.Client.Template;

public interface ITemplateValueReplacer
{
    string Replace(string valueWithVar);

    void Store(string templateKey, string value);
}