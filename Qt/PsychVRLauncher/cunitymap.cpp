#include "cunitymap.h"
#include <QApplication>
#include "cunityobject.h"
#include <QDebug>
#include<QVector3D>
//TODO NULL CHECK for no selection, stop double loads, add more settings
CUnityMap::CUnityMap(QObject *parent): QObject(parent)
{
    m_map_file = QApplication::applicationDirPath()+"/settings.ini";
    //loadSettings();
}
void CUnityMap::loadSettings()
{
    qDebug("Loading settings");
    QSettings settings(m_map_file,QSettings::NativeFormat);
    int size = settings.beginReadArray("obj");
    for(int i = 0; i < size; i++)
    {
        settings.setArrayIndex(i);
        settings.beginGroup("Attributes");
        QString name = settings.value("name").toString();
        int id = settings.value("id").toInt();
        QColor color = QColor(settings.value("color").toString());
        QImage img = QImage(settings.value("image").toString());
            settings.beginGroup("scale");
            float xs = settings.value("xs").toFloat();
            float ys = settings.value("ys").toFloat();
            float zs = settings.value("zs").toFloat();
            settings.endGroup();
            settings.beginGroup("location");
            float xp = settings.value("xp").toInt();
            float yp = settings.value("yp").toInt();
            float zp = settings.value("zp").toInt();
            settings.endGroup();
        settings.endGroup();
        m_objects.push_back(new CUnityObject(name,QVector3D(xp,yp,zp),QVector3D(xs,ys,zs),id,QString("test"),color));
    }
}

void CUnityMap::addObject(CUnityObject * obj)
{
    m_objects.push_back(obj);
}

void CUnityMap::saveSettings()
{
    QSettings settings(m_map_file,QSettings::NativeFormat);
    settings.clear();
    qDebug("Saving settings to %s", settings.fileName().toStdString().c_str());
    settings.beginWriteArray("obj");
    if(m_objects.size() == 0)
    {
        m_objects.push_back(new CUnityObject("test_object",QVector3D(1,1,1),QVector3D(1,2,3),2,QString("test.png"),QColor("#FFFFFF")));

    }
    for(int i = 0; i < (int)m_objects.size(); i++)
    {
        settings.setArrayIndex(i);
        settings.beginGroup("Attributes");
        settings.setValue("name",m_objects[i]->getName());
        settings.setValue("id",m_objects[i]->getID());
        settings.setValue("color",m_objects[i]->getColor().name());
        settings.setValue("image",m_objects[i]->getImagePath());
            settings.beginGroup("scale");
            qDebug("%f",m_objects[i]->getScale().x());
            settings.setValue("xs",m_objects[i]->getScale().x());
            settings.setValue("ys",m_objects[i]->getScale().y());
            settings.setValue("zs",m_objects[i]->getScale().z());
            settings.endGroup();
            settings.beginGroup("location");
            settings.setValue("xp",m_objects[i]->getLocation().x());
            settings.setValue("yp",m_objects[i]->getLocation().y());
            settings.setValue("zp",m_objects[i]->getLocation().z());
            settings.endGroup();
        settings.endGroup();
    }
    settings.endArray();
}

