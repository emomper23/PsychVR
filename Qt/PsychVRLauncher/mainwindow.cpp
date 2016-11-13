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
#include <QColor>
#include <QColorDialog>

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
    m_settings->setVisible(false);
    m_obj_settings->setVisible(false);
    ui->listWidget->setSelectionMode(QAbstractItemView::SingleSelection);
    //m_settings->show();
    //ui->tab_3->setEnabled(false);

    connect(ui->actionSave,SIGNAL(triggered(bool)),this,SLOT(saveFiles()));
    connect(ui->actionLoad,SIGNAL(triggered(bool)),this,SLOT(loadFiles()));
    connect(ui->actionChange_Object,SIGNAL(triggered(bool)),this,SLOT(editModel()));
    connect(m_obj_settings,SIGNAL(accepted()),this,SLOT(saveModel()));
    connect(ui->actionNew_Object,SIGNAL(triggered(bool)),this,SLOT(newModel()));
    connect(this->m_obj_settings, SIGNAL(accepted()),this,SLOT(saveModel()));
    connect(ui->pushButton, SIGNAL(clicked(bool)),this,SLOT(showSettings()));
    connect(ui->launchButton, SIGNAL(pressed()),this,SLOT(launchScene()));
    connect(ui->submit_button, SIGNAL(clicked(bool)),this,SLOT(SaveData()));
    connect(ui->pushButton, SIGNAL(clicked(bool)),this,SLOT(openWindow()));
    connect(ui->actionUser_1, SIGNAL(triggered(bool)),signalMapper,SLOT(map()));
    connect(ui->actionUser_2, SIGNAL(triggered(bool)),signalMapper,SLOT(map()));
    connect(ui->actionUser_3, SIGNAL(triggered(bool)),signalMapper,SLOT(map()));
    connect(ui->actionUser_4, SIGNAL(triggered(bool)),signalMapper,SLOT(map()));
    connect(ui->actionUser_5, SIGNAL(triggered(bool)),signalMapper,SLOT(map()));
    connect(ui->actionGuest_1, SIGNAL(triggered(bool)),signalMapper,SLOT(map()));
    connect(ui->tabWidget, SIGNAL(currentChanged(int)),this,SLOT(tabChanged(int)));

    signalMapper -> setMapping (ui->actionUser_1, 1) ;
    signalMapper -> setMapping (ui->actionUser_2, 2)  ;
    signalMapper -> setMapping (ui->actionUser_3, 3)  ;
    signalMapper -> setMapping (ui->actionUser_4, 4)  ;
    signalMapper -> setMapping (ui->actionUser_5, 5)  ;
    signalMapper -> setMapping (ui->actionGuest_1, 6)  ;
    connect (signalMapper, SIGNAL(mapped(int)), this, SLOT(changeUser(int))) ;
    //ui->graphWidget->addGraph();

    initButtons();
    loadFiles();
    readIn();

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

void MainWindow::tabChanged(int tab)
{
    if(tab == 3)
    {
        readIn();
    }
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

    QString giveStuff = createRun();

    if(ui->scene_selection->currentIndex()  == MainWindow::scene_idx_t::FEAR_OF_HEIGHTS)
    {
      command = "start ../heights.exe " + giveStuff;
    }
    else if(ui->scene_selection->currentIndex()  ==  MainWindow::scene_idx_t::SPEECH_ANXIETY)
    {
      command = "start ../speech.exe " + giveStuff;
    }
    else if(ui->scene_selection->currentIndex()  == MainWindow::scene_idx_t::TERRAIN_GENERATION)
    {
      command = "start ../anxiety.exe " + giveStuff;
    }

    ui->tab_3->setEnabled(true);

   system(command.toStdString().c_str());
}

void MainWindow::showSettings()
{
    m_settings->setupSettings(ui->userLabel->text().right(1).toInt());
    m_settings->setVisible(true);
    m_settings->show();


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

QString MainWindow::createRun()
{

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

    QJsonArray newruns;

    int runId = newruns.size() + 1;

    QJsonObject newRun
    {
        {"run", runId}
    };

    if(ui->scene_selection->currentIndex() == MainWindow::scene_idx_t::FEAR_OF_HEIGHTS)
    {
        newruns = heightScene["runs"].toArray();
        int runId = newruns.size() + 1;
        QJsonObject newRun
        {
            {"run", runId}
        };
        newruns.append(newRun);
        heightScene["runs"] = newruns;
    }
    else if(ui->scene_selection->currentIndex() == MainWindow::scene_idx_t::SPEECH_ANXIETY)
    {
        newruns = socialScene["runs"].toArray();
        int runId = newruns.size() + 1;
        QJsonObject newRun
        {
            {"run", runId}
        };
        newruns.append(newRun);
        socialScene["runs"] = newruns;
    }
    else if(ui->scene_selection->currentIndex()  == MainWindow::scene_idx_t::TERRAIN_GENERATION)
    {
        newruns = calmScene["runs"].toArray();
        int runId = newruns.size() + 1;
        QJsonObject newRun
        {
            {"run", runId}
        };
        newruns.append(newRun);
        calmScene["runs"] = newruns;
    }

    user["Heights"] = heightScene;
    user["Calm"] = calmScene;
    user["Social"] = socialScene;
    tester[usern] = user;

    if (!saveFile.open(QIODevice::WriteOnly)) {
           qWarning("Failed to save data.");
           //return false;
       }
    QJsonDocument saveDoc(tester);

    saveFile.write(saveDoc.toJson());
    saveFile.close();

    return "" + ui->userLabel->text().right(1);

}

void MainWindow::SaveData()
{

    int usernum = 0;
    int scenenum = 0;

    for(int x = 0; x < radioQs.size();x++)
    {
        if (radioQs[x]->checkedId() == -1) {
            ui->errorLabel->setText("Please Answer All Questions");
            return;
        }
    }
    ui->errorLabel->setText("");

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

    QJsonObject curRun = newruns.last().toObject();

    QJsonObject answers
    {
        {"1",radioQs[0]->checkedId()},
        {"2",radioQs[1]->checkedId()},
        {"3",radioQs[2]->checkedId()},
        {"4",radioQs[3]->checkedId()},
        {"5",radioQs[4]->checkedId()},
        {"6",radioQs[5]->checkedId()}
    };

    curRun.insert("answers",answers);
    curRun.insert("prestress", ui->stress_slider->sliderPosition());
    curRun.insert("poststress", ui->anxiety_slider->sliderPosition());
    curRun.insert("notes", ui->textEdit->toPlainText());

    newruns.removeLast();

    newruns.append(curRun);

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
        return;
    }

    usernum = ui->userLabel->text().right(1).toInt();

    QJsonObject heightScene = tester.at(usernum).toObject()["Heights"].toObject();
    QJsonObject calmScene = tester.at(usernum).toObject()["Calm"].toObject();
    QJsonObject socialScene = tester.at(usernum).toObject()["Social"].toObject();
    QJsonObject user = tester.at(usernum).toObject();

    QVector<double> stressBefore;
    QVector<double> stressAfter;
    QVector<double> stressScores;
    QVector<double> indexes;
    QVector<QString> notes;

    int score = 0;

    QJsonArray heightRuns = heightScene["runs"].toArray();

    for(int iter = 0; iter < heightRuns.size(); iter ++)
    {

        stressBefore.append(heightRuns[iter].toObject()["prestress"].toInt());
        stressAfter.append(heightRuns[iter].toObject()["poststress"].toInt());
        score = heightRuns[iter].toObject()["answers"].toObject()["1"].toInt() * -1 + 6;
        score += heightRuns[iter].toObject()["answers"].toObject()["2"].toInt();
        score += heightRuns[iter].toObject()["answers"].toObject()["3"].toInt();
        score += heightRuns[iter].toObject()["answers"].toObject()["4"].toInt();
        score += heightRuns[iter].toObject()["answers"].toObject()["5"].toInt();
        score += heightRuns[iter].toObject()["answers"].toObject()["6"].toInt();
        score = (score / 36) * 10;
        stressScores.append(score);
        indexes.append((double)iter);
        score = 0;
        notes.append(heightRuns[iter].toObject()["notes"].toString());
    }

    //---------------------------------------------------------------------------------------------------

    ui->customPlot->legend->setVisible(true);
    ui->customPlot->legend->setFont(QFont("Helvetica", 9));
    ui->customPlot->clearGraphs();
    QPen pen;
    QStringList lineNames;

    // add graphs with different line styles:

    ui->customPlot->addGraph();
    ui->customPlot->addGraph();
    ui->customPlot->addGraph();


    pen.setColor(QColor(240,0,0));

    ui->customPlot->graph(0)->setPen(pen);
    ui->customPlot->graph(0)->setName("Pre-Stress");
    ui->customPlot->graph(0)->setLineStyle((QCPGraph::LineStyle)1);
    ui->customPlot->graph(0)->setScatterStyle(QCPScatterStyle(QCPScatterStyle::ssCircle, 10));
    ui->customPlot->graph(0)->setData(indexes,stressBefore);

    pen.setColor(QColor(0,240,0));

    ui->customPlot->graph(1)->setPen(pen);
    ui->customPlot->graph(1)->setName("Post-Stress");
    ui->customPlot->graph(1)->setLineStyle((QCPGraph::LineStyle)1);
    ui->customPlot->graph(1)->setScatterStyle(QCPScatterStyle(QCPScatterStyle::ssCircle, 10));
    ui->customPlot->graph(1)->setData(indexes,stressAfter);

    pen.setColor(QColor(0,0,240));

    ui->customPlot->graph(2)->setPen(pen);
    ui->customPlot->graph(2)->setName("Scores");
    ui->customPlot->graph(2)->setLineStyle((QCPGraph::LineStyle)1);
    ui->customPlot->graph(2)->setScatterStyle(QCPScatterStyle(QCPScatterStyle::ssCircle, 10));
    ui->customPlot->graph(2)->setData(indexes,stressScores);

    ui->customPlot->setInteractions(QCP::iRangeDrag | QCP::iRangeZoom);
    ui->customPlot->xAxis->setRange(-.2,indexes.size());
    ui->customPlot->yAxis->setRange(-.2,10);


    /*

    for (int i=QCPGraph::lsNone; i<=QCPGraph::lsImpulse; ++i)
    {
      ui->customPlot->addGraph();
      pen.setColor(QColor(qSin(i*1+1.2)*80+80, qSin(i*0.3+0)*80+80, qSin(i*0.3+1.5)*80+80));
      ui->customPlot->graph()->setPen(pen);
      ui->customPlot->graph()->setName(lineNames.at(i-QCPGraph::lsNone));
      ui->customPlot->graph()->setLineStyle((QCPGraph::LineStyle)i);
      ui->customPlot->graph()->setScatterStyle(QCPScatterStyle(QCPScatterStyle::ssCircle, 5));
      // generate data:
      QVector<double> x(15), y(15);
      for (int j=0; j<15; ++j)
      {
        x[j] = j/15.0 * 5*3.14 + 0.01;
        y[j] = 7*qSin(x[j])/x[j] - (i-QCPGraph::lsNone)*5 + (QCPGraph::lsImpulse)*5 + 2;
      }
      ui->customPlot->graph()->setData(x, y);
      ui->customPlot->graph()->rescaleAxes(true);
    }

    // zoom out a bit:
    ui->customPlot->yAxis->scaleRange(1.1, ui->customPlot->yAxis->range().center());
    ui->customPlot->xAxis->scaleRange(1.1, ui->customPlot->xAxis->range().center());
    // set blank axis lines:
    ui->customPlot->xAxis->setTicks(false);
    ui->customPlot->yAxis->setTicks(true);
    ui->customPlot->xAxis->setTickLabels(false);
    ui->customPlot->yAxis->setTickLabels(true);
    // make top right axes clones of bottom left axes:
    ui->customPlot->axisRect()->setupFullAxesBox();
    */


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
    readIn();

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
