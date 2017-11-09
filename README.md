# ProductAPI
API to Manage Products on Lorien. Used to create, update, read and delete Products. Also responsible for managing its children.
## Product Data Format
These are the fields of the thing and it's constrains:
- productId: Id of the Product given by de Database.
  - Integer
  - Ignored on Create, mandatory on the other methods
- parentProducId: Id of the Product wich this thing belong to.
  - Integer
  -  Ignored on Create and Update
- productName: Name of the Product given by the user.
  - String (Up to 50 chars)
  - Mandatory
- producDescription: Free description of the Producthing.
  - String (Up to 100 chars)
  - Optional
- productGTIN: GTIN of the product according to GS1.
  - String (Up to 50 chars)
  - Optional
- enabled: Products cannot be deleted they are just disabled in the backend and dont show up in the queries.
  - Boolean
  - Mandatory
- producCode: Code that might be used by the end user to identify the Product easily.
  - String (Up to 100 chars)
  - Optional
- childrenProductsIds: List of Id of Products from which this one is parent.
  - Array Integer
  - Ignored on Create and Update
- additionalInformation: List of additional information related to de product.
  - Array AdditionalInformation
  - Optional
  
### JSON Example:
```json
{
    "productId": 2,
    "parentProducId": null,
    "producName": "Nome Teste 2",
    "producDescription": "Teste Decription",
    "producCode": "TesteCode",
    "productGTIN": "+9999999",
    "childrenProductsIds": [1,2],
    "enabled": true,
    "additionalInformation": [
    {
      "information": "Densidade",
      "value": "60"
    }]
 }
```
## URLs
- api/products/{optional=startat}{optional=quantity}
  - Get: Return List of Products
    - startat: represent where the list starts t the database (Default=0)
    - quantity: number of resuls in the query (Default=50)
  - Post: Create the Product with the JSON in the body
    - Body: Product JSON

- api/products/{id}
  - Get: Return Product with productId = ID
  - Put: Update the Product with the JSON in the body with productId = ID
    - Body: Product JSON
  - Delete: Disable Product with productId = ID

- api/products/list{productid}{productid}
    - Get: Return List of Products with productid = ID

- api/products/childrenproducts/{parentId}
  - Get: Return List of Products which the parent is parentId
  - Post: Insert the Product with the JSON in the body as child of the parent Product
    - Body: Product JSON
  - Delete: Remove Product with JSON in the body as child of parent Product.
    - Body: Product JSON
