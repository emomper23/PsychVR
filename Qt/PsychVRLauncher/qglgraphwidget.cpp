#include "qglgraphwidget.h"


void QGLGraphWidget::paintGL()
{
    qDebug("paint callback");
    QPainter painter(this);
    painter.setBrush(QBrush(QColor(255,0,0)));
    painter.setRenderHint(QPainter::Antialiasing,true);
   // painter.fillRect(10,10,100,100,QBrush(QColor(255,0,0)));
    painter.beginNativePainting();
    struct point {
      GLfloat x;
      GLfloat y;
    };
    point graph[2000];

    for(int i = 0; i < 2000; i++)
    {
      //float x = o
      graph[i].x = i*1.0;
      graph[i].y = 10.0 ;
    }
      GLuint vbo;

      glGenBuffers(1, &vbo);
      glBindBuffer(GL_ARRAY_BUFFER, vbo);
      glBufferData(GL_ARRAY_BUFFER, sizeof graph, graph, GL_STATIC_DRAW);
      glBindBuffer(GL_ARRAY_BUFFER, vbo);
      glEnableVertexAttribArray(0);
      glVertexAttribPointer(
        0,   // attribute
        2,                   // number of elements per vertex, here (x,y)
        GL_FLOAT,            // the type of each element
        GL_FALSE,            // take our values as-is
        0,                   // no space between values
        0                    // use the vertex buffer object
      );

      glDrawArrays(GL_POINTS, 0, 2000);

      glDisableVertexAttribArray(0);
      glBindBuffer(GL_ARRAY_BUFFER, 0);
    painter.endNativePainting();

}
