# Limpar a pasta de publicação
if (Test-Path "publish") {
    Remove-Item -Path "publish" -Recurse -Force
}

# Publicar para Windows x64
dotnet publish -c Release -r win-x64 --self-contained true /p:PublishSingleFile=true /p:PublishTrimmed=true -o "publish/win-x64"

# Renomear o executável para jota.exe
Move-Item "publish/win-x64/JotLang.exe" "publish/win-x64/jota.exe"

Write-Host "`nPublicação concluída! O executável está em publish/win-x64/jota.exe" 