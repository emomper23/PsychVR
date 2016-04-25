#include "qglgraphwidget.h"


void QGLGraphWidget::paintGL()
{

    QPainter paint(this);
    paint.setBrush(QBrush(QColor(1,0,0)));
    paint.drawRect(10,10,100,100);

}
