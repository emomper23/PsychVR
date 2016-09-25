#ifndef QGLGRAPHWIDGET_H
#define QGLGRAPHWIDGET_H

#include <QObject>
#include <QWidget>
#include <QOpenGLWidget>
#include <QOpenGLFunctions>
#include <QPainter>
#include <QPainterPath>
class QGLGraphWidget: public QOpenGLWidget , public QOpenGLFunctions
{
public:
    QGLGraphWidget(QWidget *parent):QOpenGLWidget(parent) { }
protected:
    void paintGL();
    void initializeGL(){initializeOpenGLFunctions();}
};

#endif // QGLGRAPHWIDGET_H
