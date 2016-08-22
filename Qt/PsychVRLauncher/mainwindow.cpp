#include "mainwindow.h"
#include "ui_mainwindow.h"
#include "cunityobject.h"
#include<QDebug>

MainWindow::MainWindow(QWidget *parent) :
    QMainWindow(parent),
    ui(new Ui::MainWindow)
{
    ui->setupUi(this);
    m_map = new CUnityMap();
    connect(ui->actionSave,SIGNAL(triggered(bool)),this,SLOT(saveFiles()));
    connect(ui->actionLoad,SIGNAL(triggered(bool)),this,SLOT(loadFiles()));
    connect(ui->actionChange_Object,SIGNAL(triggered(bool)),this,SLOT(editModel()));
    ui->listWidget->setSelectionMode(QAbstractItemView::SingleSelection);
    m_obj_settings = new EditDialog();
    m_obj_settings->setVisible(false);
}
MainWindow::~MainWindow()
{
    delete ui;
}

void MainWindow::loadFiles()
{
    m_map->loadSettings();
    for(int i = 0; i< m_map->m_objects.size()  ; i++)
    {
        qDebug()<<"test";
        qDebug()<<m_map->m_objects[i]->getName();
        QListWidgetItem* item = new QListWidgetItem(ui->listWidget);
        item->setText(m_map->m_objects[i]->getName());
        ui->listWidget->addItem(item);

    }
}
void MainWindow::saveFiles()
{
    m_map->saveSettings();
}

void MainWindow::editModel()
{
    QString selected = ui->listWidget->selectedItems().at(0)->text();
    CUnityObject * obj = NULL;
    qDebug()<< "editing" << selected;
    for(int i = 0; i < m_map->m_objects.size();i++)
    {
        if(m_map->m_objects[i]->getName() == selected)
        {
            obj = m_map->m_objects[i];
        }
    }
    m_obj_settings->setObject(obj);
    m_obj_settings->setVisible(true);

}

