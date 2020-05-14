# Report Scheduler Service

Report Scheduler Service is responsible for acquiring & managing `Report Scheduler` which represent the way system emits new `Report` instances.

## API

```
<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<< REQUEST >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>

POST: /api/report/save
Authorization: Bearer <token>
Content-Type: application/json

{
    id                      : string
    questionnaireAnswers    : []
    {
        title               : string
        order               : int
        sectionAnswers      : []
        {
            title           : string
            order           : int
            questionAnswers : []
            {
                title       : string
                order       : int
                answer      : string
                answerType  : enum
                {
                    ShortText = 0
                    LongText = 1
                    Email = 2
                    PhoneNumber = 3
                    Label = 4
                    Integer = 5
                    Decimal = 6
                    Financial = 7
                    MultipleChoiceOptionList = 8
                    SingleOptionSelect = 9
                    Boolean = 10
                }
            }

        }
    },
    tableAnswers            : [] //empty array
}

<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<< RESPONSE >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>

200 OK
```

Saves data posted to the `Report`

---

```
<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<< REQUEST >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>

POST: /api/report/pass
Authorization: Bearer <token>
Content-Type: application/json

{
    id                      : string
    questionnaireAnswers    : []
    {
        title               : string
        order               : int
        sectionAnswers      : []
        {
            title           : string
            order           : int
            questionAnswers : []
            {
                title       : string
                order       : int
                answer      : string
                answerType  : enum
                {
                    ShortText = 0
                    LongText = 1
                    Email = 2
                    PhoneNumber = 3
                    Label = 4
                    Integer = 5
                    Decimal = 6
                    Financial = 7
                    MultipleChoiceOptionList = 8
                    SingleOptionSelect = 9
                    Boolean = 10
                }
            }

        }
    },
    tableAnswers            : [] //empty array
}

<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<< RESPONSE >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>

200 OK
```

Saves final input to the `Report` (sets status `Passed`)

---

```
<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<< REQUEST >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>

GET: /api/report/org
Authorization: Bearer <token>

<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<< RESPONSE >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>

Content-Type: application/json

[
    {
        id                      : string
        assignedOrganizationId  : string
        reportStatus            : string
        deadline                : string    // stringed datetime object
    }
]
```

Returns array of some simple information about report for logged in user. For HOA using this endpoint makes no sense.

---

```
<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<< REQUEST >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>

GET: /api/report/get/{reportId}
Authorization: Bearer <token>

<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<< RESPONSE >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>

Content-Type: application/json

{
    id                      : string
    title                   : string
    reportTemplateId        : string
    assignedOrganizationId  : string
    status                  : enum
    {
        Passed
        Overdue
        InProgress
    }
    deadline                : date
    questionnaireAnswers    : []
    {
        title               : string
        order               :int
        sectionAnswers      : []
        {
            title           : string
            order           : int
            questionAnswers : []
            {
                title       : string
                order       : int
                answer      : string
                answerType  : enum
                {
                    ShortText = 0
                    LongText = 1
                    Email = 2
                    PhoneNumber = 3
                    Label = 4
                    Integer = 5
                    Decimal = 6
                    Financial = 7
                    MultipleChoiceOptionList = 8
                    SingleOptionSelect = 9
                    Boolean = 10
                }
            }

        }
    },
    tableAnswers            : [] //empty array
}
```

Returns full `Report` data model which is used both for displaying current values and for composing report itself (visually).

---

```
<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<< REQUEST >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>

GET: /api/report/info
Authorization: Bearer <token>

<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<< RESPONSE >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>

Content-Type: application/json

[
    {
        id                      : string
        assignedOrganizationId  : string
        reportStatus            : string
        deadline                : string    // stringed datetime object
    }
]
```

Returns some short info about all the reports

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

From service directory (`src/Focus.Service.ReportProcessor/` in our case) run

```sh
docker build -t focus.processor .
```

Also this can be done via general production `docker-compose` file in `src` directory. Type following command

```sh
docker-compose build processor
```
