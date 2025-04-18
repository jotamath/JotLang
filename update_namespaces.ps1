$files = Get-ChildItem -Recurse -Filter *.cs
foreach ($file in $files) {
    $content = Get-Content $file.FullName -Raw
    $content = $content -replace "namespace Jot\.", "namespace JotLang."
    $content = $content -replace "using Jot\.", "using JotLang."
    Set-Content $file.FullName $content
} 