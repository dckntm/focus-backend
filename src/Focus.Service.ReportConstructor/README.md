# Report Constructor Service

Report Constructor Service is responsible for acquiring & managing `Report Template`s which represent some abstraction over structure of `Report` instance.

## API

> Currently we work only with `Questionnaire Module Template`s so in request structure we provide information only about then

```
<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<< REQUEST >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>

POST: /api/report/template
Authorization: Bearer <token>
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
Authorization: Bearer <token>
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
Authorization: Bearer <token>

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

```
<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<< REQUEST >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>

GET: /api/report/template/stats
Authorization: Bearer <token>

<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<< RESPONSE >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>

Content-Type: application/json
{
    "totalTemplates"    : int,
}
```

Returns basic statistics about `Report Templates`s stored in service

---

## Docker

Application is deployed with Docker based on `focus.common` image which should be build first.

### Building common image

Build common image by running

```sh
docker build -t focus.common .
```

being in `src` folder of repository

### Building Focus Report Template Service image

From service directory (`src/Focus.Service.ReportConstructor/` in our case) run

```sh
docker build -t focus.constructor .
```

Also this can be done via general production `docker-compose` file in `src` directory. Type following command

```sh
docker-compose build constructor
```