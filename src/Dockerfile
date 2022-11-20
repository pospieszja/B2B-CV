FROM microsoft/dotnet:2.1-aspnetcore-runtime AS base
WORKDIR /app

FROM microsoft/dotnet:2.1-sdk AS build
WORKDIR /src
COPY ["HortimexB2B.Web/HortimexB2B.Web.csproj", "HortimexB2B.Web/"]
COPY ["HortimexB2B.Infrastructure/HortimexB2B.Infrastructure.csproj", "HortimexB2B.Infrastructure/"]
COPY ["HortimexB2B.Core/HortimexB2B.Core.csproj", "HortimexB2B.Core/"]
RUN dotnet restore "HortimexB2B.Web/HortimexB2B.Web.csproj"
COPY . .
WORKDIR "/src/HortimexB2B.Web"
RUN dotnet build "HortimexB2B.Web.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "HortimexB2B.Web.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENV ASPNETCORE_ENVIRONMENT Production
ENTRYPOINT ["dotnet", "HortimexB2B.Web.dll"]