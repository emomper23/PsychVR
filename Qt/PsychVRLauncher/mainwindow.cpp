#include "mainwindow.h"
#include "ui_mainwindow.h"
#include "cunityobject.h"
#include<QDebug>
#include<QProcess>
//#include<poppler/qt5/poppler-qt5.h>
#include<QAbstractButton>

#include <QSlider>
#include <QButtonGroup>
#include <QFile>
#include <QJsonDocument>
#include <QJsonObject>
#include <QJsonValue>
#include <QByteArray>
#include <QJsonArray>
#include <QSignalMapper>

MainWindow::MainWindow(QWidget *parent) :
    QMainWindow(parent),
    ui(new Ui::MainWindow)
{
    QSignalMapper* signalMapper = new QSignalMapper (this) ;
    ui->setupUi(this);
    m_map_list = (CUnityMap**) malloc(sizeof(CUnityMap * )*NUM_MAPS);
    m_map_list[0]= new CUnityMap("FearOfHeights");
    m_map_list[1]= new CUnityMap("SpeechAnxiety");
    m_map_list[2]= new CUnityMap("CalmingEnvironment");
    m_obj_settings = new EditDialog();
    m_settings = new settings();
    m_obj_settings->setVisible(false);
    ui->listWidget->setSelectionMode(QAbstractItemView::SingleSelection);
    m_settings->show();
    //ui->tab_3->setEnabled(false);

    connect(ui->actionSave,SIGNAL(triggered(bool)),this,SLOT(saveFiles()));
    connect(ui->actionLoad,SIGNAL(triggered(bool)),this,SLOT(loadFiles()));
    connect(ui->actionChange_Object,SIGNAL(triggered(bool)),this,SLOT(editModel()));
    connect(m_obj_settings,SIGNAL(accepted()),this,SLOT(saveModel()));
    connect(ui->actionNew_Object,SIGNAL(triggered(bool)),this,SLOT(newModel()));
    connect(this->m_obj_settings, SIGNAL(accepted()),this,SLOT(saveModel()));
    connect(ui->launchButton, SIGNAL(pressed()),this,SLOT(launchScene()));
    connect(ui->submit_button, SIGNAL(clicked(bool)),this,SLOT(SaveData()));
    connect(ui->pushButton, SIGNAL(clicked(bool)),this,SLOT(openWindow()));
    connect(ui->actionUser_1, SIGNAL(triggered(bool)),signalMapper,SLOT(map()));
    connect(ui->actionUser_2, SIGNAL(triggered(bool)),signalMapper,SLOT(map()));
    connect(ui->actionUser_3, SIGNAL(triggered(bool)),signalMapper,SLOT(map()));
    connect(ui->actionUser_4, SIGNAL(triggered(bool)),signalMapper,SLOT(map()));
    connect(ui->actionUser_5, SIGNAL(triggered(bool)),signalMapper,SLOT(map()));
    connect(ui->actionGuest_1, SIGNAL(triggered(bool)),signalMapper,SLOT(map()));

    signalMapper -> setMapping (ui->actionUser_1, 1) ;
    signalMapper -> setMapping (ui->actionUser_2, 2)  ;
    signalMapper -> setMapping (ui->actionUser_3, 3)  ;
    signalMapper -> setMapping (ui->actionUser_4, 4)  ;
    signalMapper -> setMapping (ui->actionUser_5, 5)  ;
    signalMapper -> setMapping (ui->actionGuest_1, 6)  ;
    connect (signalMapper, SIGNAL(mapped(int)), this, SLOT(changeUser(int))) ;


    initButtons();
    loadFiles();

    //Poppler::Document * doc = Poppler::Document::load("/home/emomper/Documents/exam.pdf");
    //QImage img = doc->page(0)->renderToImage();
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

    int usernum = 0;
    int scenenum = 0;


    for(int x = 0; x < radioQs.size();x++)
    {
        if (radioQs[x]->checkedId() == -1) {
            //ERROR HERE
        }
    }

    QJsonArray tester;

    QString filename = QApplication::applicationDirPath() + "/save.json";
    QFile saveFile(filename);
    if (!saveFile.open(QIODevice::ReadOnly)) {
           qWarning("Failed to save data.");
           //return false; c
            tester = makeJson();
    }
    else
    {
        QByteArray saveData = saveFile.readAll();
        QJsonDocument loadDoc(QJsonDocument::fromJson(saveData));
        tester = loadDoc.array();
    }

    saveFile.close();

    int usern = ui->userLabel->text().right(1).toInt();


    QJsonObject heightScene = tester.at(usern).toObject()["Heights"].toObject();
    QJsonObject calmScene = tester.at(usern).toObject()["Calm"].toObject();
    QJsonObject socialScene = tester.at(usern).toObject()["Social"].toObject();
    QJsonObject user = tester.at(usern).toObject();

    qDebug() << "check2";
    qDebug() << heightScene.isEmpty();

    QJsonArray newruns;

    if(ui->scene_selection->currentIndex() == 0)
    {
        newruns = heightScene["runs"].toArray();
    }
    else if(ui->scene_selection->currentIndex() == 1)
    {
        newruns = socialScene["runs"].toArray();
    }
    else
    {
        newruns = calmScene["runs"].toArray();
    }

    QJsonObject answers
    {
        {"1",radioQs[0]->checkedId()},
        {"2",radioQs[1]->checkedId()},
        {"3",radioQs[2]->checkedId()},
        {"4",radioQs[3]->checkedId()},
        {"5",radioQs[4]->checkedId()},
        {"6",radioQs[5]->checkedId()}
    };

    QJsonObject newRun
    {
        {"run", newruns.size() + 1},
        {"prestress", ui->stress_slider->sliderPosition()},
        {"poststress", ui->anxiety_slider->sliderPosition()},
        {"answers", answers},
        {"notes", ui->textEdit->toPlainText()}
    };

    newruns.append(newRun);

    if(ui->scene_selection->currentIndex() == 0)
    {
        heightScene["runs"] = newruns;
    }
    else if(ui->scene_selection->currentIndex() == 1)
    {
        socialScene["runs"] = newruns;
    }
    else
    {
        calmScene["runs"] = newruns;
    }

    user["Heights"] = heightScene;
    user["Calm"] = calmScene;
    user["Social"] = socialScene;

    tester[usern] = user;

    qDebug() << filename;

    if (!saveFile.open(QIODevice::WriteOnly)) {
           qWarning("Failed to save data.");
           //return false;
       }
    QJsonDocument saveDoc(tester);

    saveFile.write(saveDoc.toJson());

    saveFile.close();

    readIn();

}

void MainWindow::readIn()
{

    int usernum = 0;
    int scenenum = 0;

    QString filename = QApplication::applicationDirPath() + "/save.json";
    QFile saveFile(filename);
    if (!saveFile.open(QIODevice::ReadOnly)) {
           qWarning("Failed to save data.");
           //return false;
       }

    QByteArray saveData = saveFile.readAll();
    QJsonDocument loadDoc(QJsonDocument::fromJson(saveData));
    QJsonArray tester(loadDoc.array());
    saveFile.close();

    if (tester.isEmpty())
    {
        tester = makeJson();
    }

    int usern = ui->userLabel->text().right(1).toInt();


    QJsonObject heightScene = tester.at(usern).toObject()["Heights"].toObject();
    QJsonObject calmScene = tester.at(usern).toObject()["Calm"].toObject();
    QJsonObject socialScene = tester.at(usern).toObject()["Social"].toObject();
    QJsonObject user = tester.at(usern).toObject();

    QJsonObject settings;

    if(ui->scene_selection->currentIndex() == 0)
    {
        qDebug() << heightScene.value("Settings").toObject().value("Lift_Pos").toInt();
        qDebug() << heightScene.value("Settings").toObject().value("Day").toInt();
    }
    else if(ui->scene_selection->currentIndex() == 1)
    {
        qDebug() << socialScene.value("Settings").toObject().value("PPT").toString();
        qDebug() << socialScene.value("Settings").toObject().value("Position").toInt();
    }
    else
    {
        qDebug() << calmScene["Settings"].toObject();
    }

}

void MainWindow::changeUser(int userNum)
{
    QString name =  "User";
    name.append(QString::number(userNum));
    qDebug() << name.end();
    ui->userLabel->setText(name);
    if(userNum == 6)
    {
        ui->userLabel->setText(" Guest ");
    }
}

QJsonArray MainWindow::makeJson()
{
    QJsonObject fakesettings;
    QJsonArray fakeruns;

    QJsonObject scene{
        {"Settings", fakesettings},
        {"runs", fakeruns}
    };

    QJsonObject run{
        {"Heights", scene},
        {"Social", scene},
        {"Calm", scene}
    };

    QJsonArray runs{run,run,run,run,run,run};

    return runs;

}
