FROM focus.common:latest AS build-env
WORKDIR /app

RUN mkdir src
COPY . ./src/
# RUN dotnet restore src/Api
RUN dotnet publish src/Api -c Release -o out

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1
COPY --from=build-env /app/out .

ENTRYPOINT ["dotnet", "Focus.Service.Identity.Api.dll"]