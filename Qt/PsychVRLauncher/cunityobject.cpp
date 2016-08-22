#include "cunityobject.h"
#include <QString>
#include <QDebug>
CUnityObject::CUnityObject(QString name, QVector3D loc, QVector3D scale, int id, QString img_path, QColor color, QObject *parent):m_id(0),QObject(parent)
{
    m_name = name;
    m_location = loc;
    m_scale = scale;
    m_id = id;
    m_color = color;
    m_image_path = img_path;
}

