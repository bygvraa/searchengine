import os


PATH = os.path.dirname(os.path.abspath(__file__))

services = {
    "BlazorWebClient": f"dotnet run --project {PATH}/Client --urls http://localhost:5076",
    "LoadBalancer": f"dotnet run --project {PATH}/LoadBalancer --urls http://localhost:5238",
    "DatabaseServer": f"dotnet run --project {PATH}/Database --urls http://localhost:5097",
    "LogicServer1": f"dotnet run --project {PATH}/Server --urls https://localhost:7233",
    "LogicServer2": f"dotnet run --project {PATH}/Server --urls https://localhost:7234"
}

for service, command in services.items():
    os.system(f'start "{service}" cmd /k {command}')
