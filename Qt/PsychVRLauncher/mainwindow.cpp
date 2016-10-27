#include "mainwindow.h"
#include "ui_mainwindow.h"
#include "cunityobject.h"
#include<QDebug>
#include<QProcess>
#include<poppler/qt5/poppler-qt5.h>
#include<QAbstractButton>
#include <QButtonGroup>

MainWindow::MainWindow(QWidget *parent) :
    QMainWindow(parent),
    ui(new Ui::MainWindow)
{
    ui->setupUi(this);
    m_map_list = (CUnityMap**) malloc(sizeof(CUnityMap * )*NUM_MAPS);
    m_map_list[0]= new CUnityMap("FearOfHeights");
    m_map_list[1]= new CUnityMap("SpeechAnxiety");
    m_map_list[2]= new CUnityMap("CalmingEnvironment");
    m_obj_settings = new EditDialog();
    m_obj_settings->setVisible(false);
    ui->listWidget->setSelectionMode(QAbstractItemView::SingleSelection);

    ui->tab_3->setEnabled(false);

    connect(ui->actionSave,SIGNAL(triggered(bool)),this,SLOT(saveFiles()));
    connect(ui->actionLoad,SIGNAL(triggered(bool)),this,SLOT(loadFiles()));
    connect(ui->actionChange_Object,SIGNAL(triggered(bool)),this,SLOT(editModel()));
    connect(m_obj_settings,SIGNAL(accepted()),this,SLOT(saveModel()));
    connect(ui->actionNew_Object,SIGNAL(triggered(bool)),this,SLOT(newModel()));
    connect(this->m_obj_settings, SIGNAL(accepted()),this,SLOT(saveModel()));
    connect(ui->launchButton, SIGNAL(pressed()),this,SLOT(launchScene()));
    connect(ui->submit_button, SIGNAL(clicked(bool)),this,SLOT(SaveData()));
    initButtons();
    loadFiles();

    Poppler::Document * doc = Poppler::Document::load("/home/emomper/Documents/exam.pdf");
    QImage img = doc->page(0)->renderToImage();
    //ui->openGLWidget->setImage
}
MainWindow::~MainWindow()
{
    delete ui;
}

CUnityMap * MainWindow::getMap()
{
    qDebug()<<ui->scene_selection->currentIndex();
    return  m_map_list[ui->scene_selection->currentIndex()];
}
void MainWindow::loadFiles()
{
    this->getMap()->loadSettings();
    for(int i = 0; i< this->getMap()->m_objects.size()  ; i++)
    {
        qDebug()<<"test";
        qDebug()<<this->getMap()->m_objects[i]->getName();
        QListWidgetItem* item = new QListWidgetItem(ui->listWidget);
        item->setText(this->getMap()->m_objects[i]->getName());
        ui->listWidget->addItem(item);

    }
}
void MainWindow::saveFiles()
{
    this->getMap()->saveSettings();
}

void MainWindow::editModel()
{



    if(ui->listWidget->selectedItems().size() == 0)
            return;
        QString selected = ui->listWidget->selectedItems().at(0)->text();
    CUnityObject * obj = NULL;


    qDebug()<< "editing" << selected;
    for(int i = 0; i < this->getMap()->m_objects.size();i++)
    {
        if(this->getMap()->m_objects[i] != NULL && this->getMap()->m_objects[i]->getName() == selected)
        {
            obj = this->getMap()->m_objects[i];
        }
    }
    if(obj == NULL)
    {
        qDebug("please select an object");
        return;
    }
    m_obj_settings->setObject(obj);
    m_obj_settings->setVisible(true);

}

void MainWindow::saveModel()
{
    m_obj_settings->saveObject();
    saveFiles();
}
void MainWindow::newModel()
{
    CUnityObject * obj = new CUnityObject(this->getMap()->m_objects.size());
    this->getMap()->addObject(obj);

}
void MainWindow::launchScene()
{
    QProcess proc;
    QString command = "";
    if(ui->scene_selection->currentIndex()  == MainWindow::scene_idx_t::FEAR_OF_HEIGHTS)
    {
      command = "C:/Users/EricM/Desktop/a.exe test_args";
    }
    else if(ui->scene_selection->currentIndex()  ==  MainWindow::scene_idx_t::SPEECH_ANXIETY)
    {
      command = "C:/Users/EricM/Desktop/test.exe";
    }
    else if(ui->scene_selection->currentIndex()  == MainWindow::scene_idx_t::TERRAIN_GENERATION)
    {
      command = "start C:/Users/EricM/Desktop/test.exe";
    }

    ui->tab_3->setEnabled(true);

   system(command.toStdString().c_str());
}

void MainWindow::initButtons()
{

    radioQs.push_back(ui->buttonGroup);
    radioQs.push_back(ui->buttonGroup_2);
    radioQs.push_back(ui->buttonGroup_3);
    radioQs.push_back(ui->buttonGroup_4);
    radioQs.push_back(ui->buttonGroup_5);
    radioQs.push_back(ui->buttonGroup_6);
    for(int x = 0; x < radioQs.size();x++)
    {
        for(int i = 0; i < radioQs[x]->buttons().count();i++)
        {
            radioQs[x]->setId(radioQs[x]->buttons()[i],i);
        }
    }
}

void MainWindow::SaveData()
{
    for(int x = 0; x < radioQs.size();x++)
    {
         qDebug() << radioQs[x]->checkedId();

    }

}
