FROM mcr.microsoft.com/dotnet/sdk:9.0

WORKDIR /app

COPY . ./

EXPOSE 5225

CMD ["dotnet", "watch", "run", "--urls", "http://+:5225"]