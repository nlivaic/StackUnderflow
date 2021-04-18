﻿# Usage: migrations_su.ps1 <previous_migration> <next_migration_number> <next_migration_name>
# Usage: migrations_su.ps1 '0001_Initial' '0003_StudycastIntegration'
$previous_migration=$args[0]
$next_migration_name=$args[1]
$full_script_path="../StackUnderflow.Migrations/StackUnderflowScripts/" + $next_migration_name + ".sql"
cd ../StackUnderflow.Data
dotnet ef migrations add $next_migration_name --startup-project ../StackUnderflow.Api/StackUnderflow.Api.csproj --context StackUnderflowDbContext
dotnet ef migrations script --startup-project ../StackUnderflow.Api/StackUnderflow.Api.csproj --context StackUnderflowDbContext $previous_migration $next_migration_name -o $full_script_path
cd ../StackUnderflow.Migrations