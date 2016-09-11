#ifndef EDITDIALOG_H
#define EDITDIALOG_H

#include <QDialog>
#include "cunityobject.h"

namespace Ui {
class EditDialog;
}

class EditDialog : public QDialog
{
    Q_OBJECT

public:
    explicit EditDialog(QWidget *parent = 0);
    QColor getColor();
    ~EditDialog();
     void setObject(CUnityObject *obj);

public slots:
 void saveObject();

private:
    Ui::EditDialog *ui;
    CUnityObject * m_object;
};

#endif // EDITDIALOG_H
