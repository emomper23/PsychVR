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
    explicit CUnityObject(int id = 0);
    ~CUnityObject(){}

    QString getName(){return m_name;}
    QVector3D getLocation(){return m_location;}
    QColor getColor(){return m_color;}
    QString getImagePath(){return m_image_path;}
    int getID(){return m_id;}
    QVector3D getScale(){return m_scale;}

    void setName(QString name){m_name = name;}
    void setLocation(QVector3D loc ){m_location = loc;}
    void setColor(QColor col){ m_color = col;}
    void setImagePath(QString str){ m_image_path = str;}
    void setID(int id){m_id = id;}
    void setScale(QVector3D scale){ m_scale = scale;}

private:
    QString m_name;
    QVector3D m_location;
    QVector3D m_scale;
    float m_id;
    QString m_image_path;
    QColor m_color;
};


#endif // CUNITYOBJECT_H
