#ifndef CUNITYMAP_H
#define CUNITYMAP_H
#include "cunityobject.h"
#include <QSettings>
#include<QObject>
class CUnityMap : public QObject
{
    Q_OBJECT
public:
    explicit CUnityMap(QObject *parent = 0);
     std::vector<CUnityObject*> m_objects;
    ~CUnityMap(){}
     void addObject(CUnityObject * obj);
public slots:
    void loadSettings();
    void saveSettings();
private:
    QString m_map_file;
};



#endif // CUNITYMAP_H
