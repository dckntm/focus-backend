# Report Constructor Service

Report Constructor Service is responsible for acquiring & managing `Report Template`s which represent some abstraction over structure of `Report` instance.

## API

> Currently we work only with `Questionnaire Module Template`s so in request structure we provide information only about then

```
<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<< REQUEST >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>

POST: /api/report/template/
Content-Type: application/json
{
    "id"                            : string,   // empty ""
    "title"                         : string,   // not empty, null or whitespace
    "questionnaires"                : []QuestionnaireModuleTemplate
    {
        "title"                     : string,   // not empty, null or whitespace
        "order"                     : int,      // any non negative
        "sections"                  : []SectionTemplate
        {
            "title"                 : string,   // not empty, null or whitespace
            "order"                 : int,      // any non negative
            "repeatable"            : bool,
            "questions"             : []QuestionTemplate
            {
                "questionText"      : string,   // not empty, null or whitespace
                "inputType"         : InputType
                {
                    ShortText,
                    LongText,
                    Email,
                    PhoneNumber,
                    Label,
                    Integer,
                    Decimal,
                    Financial,
                    MultipleChoiceOptionList,
                    SingleOptionSelect,
                    Boolean
                }
            }
        }
      }
    "tables"                        : []TableModuleTemplate
}

<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<< RESPONSE >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>

stringed ObjectId (MongoDB ID type)
```

Creates `Report Template` entity in database

---

```
<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<< REQUEST >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>

GET: /api/report/template/{id}
where id : stringed ObjectId (MongoDB ID type)

<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<< RESPONSE >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>

Content-Type: application/json
{
    "id"                            : string,   // stringed ObjectId
    "title"                         : string,
    "questionnaires"                : []QuestionnaireModuleTemplate
    {
        "title"                     : string,
        "order"                     : int,
        "sections"                  : []SectionTemplate
        {
            "title"                 : string,
            "order"                 : int,
            "repeatable"            : bool,
            "questions"             : []QuestionTemplate
            {
                "questionText"      : string,
                "inputType"         : InputType
                {
                    ShortText,
                    LongText,
                    Email,
                    PhoneNumber,
                    Label,
                    Integer,
                    Decimal,
                    Financial,
                    MultipleChoiceOptionList,
                    SingleOptionSelect,
                    Boolean
                }
            }
        }
      }
    "tables"                        : []TableModuleTemplate
}
```

Returns same dto model for `Report Template`

---

```
<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<< REQUEST >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>

GET: /api/report/template/info

<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<< RESPONSE >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>

Content-Type: application/json
[
    {
        "id"        : string,
        "title"     : string
    }
]
```

Returns an array of `Report Template` short infos

---

## Deploy

Application is deployed with Docker based on `focus_common` image which should be build first.

### Building common image

Build common image by running

```sh
docker build -t focus_common .
```

being in `src` folder of repository

### Building Focus Report Template Service image

From service directory (`src/Focus.Service.ReportConstructor/` in our case) run

```sh
docker -t focus_constructor .
```

### Local build

> This kind of build is for backend developers

When building locally it's considered that we run this service individually & no other services were deployed on the same machine at the same time. To deploy locally we run

```sh
docker-compose -f docker-compose.local.yml up
```

from service folder (`src/Focus.Service.ReportConstructor/` in our case).

| Container Name    | Host              | Port        | Description                                                       |
| ----------------- | ----------------- | ----------- | ----------------------------------------------------------------- |
| `rcs_storage`     | `rcs_storage`     | 27017:27017 | MongoDB database where data is stored                             |
| `rcs_storage_gui` | `rcs_storage_gui` | 8081:8081   | MongoDB GUI interface that allows us to observe MongoDB databases |

In this case we run our service on our own via `dotnet run`

### Development build

> This kind of build is for frontend developers

When building for development we also consider that services are ran individually. Go to the service folder & run

```sh
docker-compose -f docker-compose.dev.yml up
```

| Container Name    | Host              | Port        | Description                                                       |
| ----------------- | ----------------- | ----------- | ----------------------------------------------------------------- |
| `rcs_storage`     | `rcs_storage`     | 27017:27017 | MongoDB database where data is stored                             |
| `rcs_storage_gui` | `rcs_storage_gui` | 8081:8081   | MongoDB GUI interface that allows us to observe MongoDB databases |
| `rcs`             | `localhost`       | 5000:5000   | Report Constructor Service instance                               |

### Production build

> TODO: add production build & description
