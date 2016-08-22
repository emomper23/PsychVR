#include "editdialog.h"
#include "ui_editdialog.h"

EditDialog::EditDialog(QWidget *parent) :
    QDialog(parent),
    ui(new Ui::EditDialog)
{
    ui->setupUi(this);
}

EditDialog::~EditDialog()
{
    delete ui;
}

void EditDialog::setObject(CUnityObject *obj)
{
    ui->txt_name->setText( obj->getName());
    ui->txt_img_name->setText(obj->getImagePath());
    ui->txt_color_name->setText(obj->getColor().name());
    ui->x_pos->setValue(obj->getLocation().x());
    ui->y_pos->setValue(obj->getLocation().y());
    ui->z_pos->setValue(obj->getLocation().z());
    ui->scale_x->setValue(obj->getScale().x());
    ui->scale_y->setValue(obj->getScale().y());
    ui->scale_z->setValue(obj->getScale().z());
}
