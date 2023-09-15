FROM mcr.microsoft.com/dotnet/sdk:6.0 AS base
RUN apt-get update && \
    apt-get install -y --allow-unauthenticated \
		libleptonica-dev \
		libc6-dev \
        libgdiplus \
        libx11-dev \
		libtesseract-dev && \
    apt-get clean && \
    rm -rf /var/lib/apt/lists/*
COPY ./tessdata /usr/share/tesseract-ocr/4.00/tessdata/

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /App
COPY . .
RUN dotnet restore && \
    dotnet publish -c Release -o out

FROM base AS final
WORKDIR /App/x64
RUN ln -s /usr/lib/x86_64-linux-gnu/liblept.so.5 libleptonica-1.82.0.so && \
    ln -s /usr/lib/x86_64-linux-gnu/libtesseract.so.4.0.1 libtesseract50.so
	
WORKDIR /App
COPY --from=build /App/out .
ENTRYPOINT ["dotnet", "OcrInvoiceBackend.API.dll"]
