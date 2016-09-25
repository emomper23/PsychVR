#ifndef CUSERSTAT_H
#define CUSERSTAT_H

#include <QObject>

class CUserStat : public QObject
{
    Q_OBJECT
public:
    explicit CUserStat(QObject *parent = 0);

signals:

public slots:
};

#endif // CUSERSTAT_H