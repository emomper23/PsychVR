#ifndef CUNITYOBJECT_H
#define CUNITYOBJECT_H
#include <QPoint>
#include <QImage>
#include <QColor>
#include<QObject>
#include <qvector3d.h>
class CUnityObject : public QObject
{
    Q_OBJECT
public:
    explicit CUnityObject(QString name, QVector3D loc, QVector3D scale, int id, QString img_path, QColor color, QObject *parent = 0);
    ~CUnityObject(){}
    //~CUnityObject(){];
    QString getName(){return m_name;}
    QVector3D getLocation(){return m_location;}
    QColor getColor(){return m_color;}
    QString getImagePath(){return m_image_path;}
    int getID(){return m_id;}
    QVector3D getScale(){return m_scale;}

private:
    QString m_name;
    QVector3D m_location;
    QVector3D m_scale;
    float m_id;
    QString m_image_path;
    QColor m_color;
};


#endif // CUNITYOBJECT_H
