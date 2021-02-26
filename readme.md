### Getting Started

    cd ./src/StackUnderflow.Api
    dotnet user-secrets set ConnectionStrings:StackUnderflowDbConnection <connection_string_here>
    dotnet user-secrets set IdP:Authority <Identity_Server_Uri_here>
    dotnet user-secrets set IdP:ApiName	<Api_Resource_Name_here>

    cd ../StackUnderflow.Identity
    dotnet user-secrets set Facebook:ClientId <Facebook_Login_Client_Id_here>
    dotnet user-secrets set Facebook:ClientSecret <Facebook_Login_Client_Secret_here>
    dotnet user-secrets set ConnectionStrings:StackUnderflowIdentityDb <connection_string_here>