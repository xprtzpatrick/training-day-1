# Docker commands

### Pull image:
`docker pull mcr.microsoft.com/mssql/server:2022-latest`

### Run SQL server image:
`docker run -e "ACCEPT_EULA=Y" -e "MSSQL_SA_PASSWORD=AmazingPassword#132" -p 1433:1433 --name sql1 --hostname sql1 -d mcr.microsoft.com/mssql/server:2022-latest`

# Dotnet EFCore commands (run from Application folder / startup project)

### Add migration:
`dotnet ef migrations add "Initial" --project ..\Xprtz.Training.Infra.EfCore`

### Update SQL database:
`dotnet ef database update`