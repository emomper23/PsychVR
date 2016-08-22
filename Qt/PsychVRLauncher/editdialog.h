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
    ~EditDialog();
     void setObject(CUnityObject *obj);
//public slots:


private:
    Ui::EditDialog *ui;
};

#endif // EDITDIALOG_H
