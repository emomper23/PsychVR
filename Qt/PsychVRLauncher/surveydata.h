#ifndef SURVEYDATA_H
#define SURVEYDATA_H

#include <QObject>

class SurveyData : public QObject
{
    Q_OBJECT

public:
    SurveyData();

public slots:
    void saveData();

};

#endif // SURVEYDATA_H
