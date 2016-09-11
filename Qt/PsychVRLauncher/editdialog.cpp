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
    m_object = obj;
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
void EditDialog::saveObject()
{
    qDebug("saving...");
    m_object->setColor(ui->txt_color_name->text());
    m_object->setName( ui->txt_name->text());
    m_object->setImagePath(ui->txt_img_name->text());
    m_object->setLocation(QVector3D(ui->x_pos->value(),ui->y_pos->value(),ui->z_pos->value()));
    m_object->setScale(QVector3D(ui->scale_x->value(),ui->scale_y->value(),ui->scale_z->value()));
}
QColor EditDialog::getColor()
{
    return ui->txt_color_name->text();
}
