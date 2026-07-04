#!/usr/bin/env bash
# cURL examples for InventorySystemPharma API
# Replace BASE_URL and TOKEN as needed.

BASE_URL="http://localhost:5000"
TOKEN="REPLACE_WITH_JWT"

# 1) Authenticate (get JWT)
# Adjust route/payload to match your auth implementation
curl -v -X POST "$BASE_URL/api/auth/login" \
  -H "Content-Type: application/json" \
  -d '{"username":"admin","password":"password"}'

# Example response: { "token": "<JWT>" }
# Save the token and set TOKEN env var before running the protected requests:
# export TOKEN=<JWT>

# 2) Create a medicine (POST)
curl -v -X POST "$BASE_URL/api/medicines" \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer $TOKEN" \
  -d '{
	"name": "Paracetamol",
	"sku": "PARA100",
	"quantity": 100,
	"price": 5.99,
	"expiryDate": "2026-12-31"
  }'

# 3) List medicines (GET)
curl -v -X GET "$BASE_URL/api/medicines" \
  -H "Accept: application/json" \
  -H "Authorization: Bearer $TOKEN"

# 4) Get a single medicine by id (GET)
# Replace {id} with actual id
curl -v -X GET "$BASE_URL/api/medicines/{id}" \
  -H "Authorization: Bearer $TOKEN"

# 5) Update a medicine (PUT)
curl -v -X PUT "$BASE_URL/api/medicines/{id}" \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer $TOKEN" \
  -d '{
	"name": "Paracetamol 500mg",
	"sku": "PARA100",
	"quantity": 150,
	"price": 6.49,
	"expiryDate": "2027-06-30"
  }'

# 6) Delete a medicine (DELETE)
curl -v -X DELETE "$BASE_URL/api/medicines/{id}" \
  -H "Authorization: Bearer $TOKEN"

# 7) Stock operations
# Stock in
curl -v -X POST "$BASE_URL/api/medicines/{id}/stock/in" \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer $TOKEN" \
  -d '{"quantity":50, "note":"Shipment"}'

# Stock out
curl -v -X POST "$BASE_URL/api/medicines/{id}/stock/out" \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer $TOKEN" \
  -d '{"quantity":10, "note":"Sale"}'

# 8) Upload image (multipart/form-data)
# Replace /path/to/file.jpg and {id}
curl -v -X POST "$BASE_URL/api/medicines/{id}/image" \
  -H "Authorization: Bearer $TOKEN" \
  -F "file=@/path/to/image.jpg"

# Notes:
# - If running on Windows PowerShell, use single quotes for JSON payloads or escape double quotes.
# - Adjust endpoints, field names and authentication routes to match your implementation.
# - You can save this script and run: bash docs/curl_commands.sh (or run individual commands after exporting TOKEN)
