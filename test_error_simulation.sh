#!/bin/bash

# Simple test script to verify error simulation functionality
echo "Testing SAP Mock Error Simulation..."

# Test 1: Timeout error
echo "Test 1: Timeout error"
response=$(curl -s -w "%{http_code}" -H "X-SAP-Mock-Error: Timeout" http://localhost:5204/api/ERP01/MM/materials/MATERIAL-001)
if [[ "$response" == *"408"* ]]; then
    echo "✓ Timeout error test passed"
else
    echo "✗ Timeout error test failed"
fi

# Test 2: Authorization error
echo "Test 2: Authorization error"
response=$(curl -s -w "%{http_code}" -H "X-SAP-Mock-Error: Authorization" http://localhost:5204/api/ERP01/MM/materials/MATERIAL-001)
if [[ "$response" == *"401"* ]]; then
    echo "✓ Authorization error test passed"
else
    echo "✗ Authorization error test failed"
fi

# Test 3: Business error
echo "Test 3: Business error"
response=$(curl -s -w "%{http_code}" -H "X-SAP-Mock-Error: Business" http://localhost:5204/api/ERP01/MM/materials/MATERIAL-001)
if [[ "$response" == *"400"* ]]; then
    echo "✓ Business error test passed"
else
    echo "✗ Business error test failed"
fi

# Test 4: System error
echo "Test 4: System error"
response=$(curl -s -w "%{http_code}" -H "X-SAP-Mock-Error: System" http://localhost:5204/api/ERP01/MM/materials/MATERIAL-001)
if [[ "$response" == *"500"* ]]; then
    echo "✓ System error test passed"
else
    echo "✗ System error test failed"
fi

# Test 5: Normal operation (no error header)
echo "Test 5: Normal operation"
response=$(curl -s -w "%{http_code}" http://localhost:5204/api/ERP01/MM/materials/MATERIAL-001)
if [[ "$response" == *"200"* ]]; then
    echo "✓ Normal operation test passed"
else
    echo "✗ Normal operation test failed"
fi

echo "All tests completed!"