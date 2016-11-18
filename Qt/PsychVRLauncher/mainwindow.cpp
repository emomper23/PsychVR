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
    //ui->gridLayout_3->setEnabled(false);

    ui->coverlabel->hide();


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
    else if(tab == 2)
    {
        if(ui->scene_selection->currentIndex()  == MainWindow::scene_idx_t::TERRAIN_GENERATION)
        {
            ui->coverlabel->show();
        }
        else if(ui->scene_selection->currentIndex() == MainWindow::scene_idx_t::FEAR_OF_HEIGHTS)
        {
            ui->coverlabel->hide();
            ui->quest1->setText("My fear of heights affects\n my day to day life");
            ui->quest2->setText("I remained calm as I went up");
            ui->quest3->setText("I remained calm as I went down");
            ui->quest4->setText("I feel proud of the height I reached");
            ui->quest5->setText("I feel good about my performance");
            ui->quest6->setText("I am better able to cope\nwith my fear than before");
        }
        else if(ui->scene_selection->currentIndex() == MainWindow::scene_idx_t::SPEECH_ANXIETY)
        {
            ui->coverlabel->hide();
            ui->quest1->setText("My speech anxiety affects\nmy day to day life");
            ui->quest2->setText("I remained calm during the presentation");
            ui->quest3->setText("I was able to focus on the presenation");
            ui->quest4->setText("I remained calm while looking\nat the audience");
            ui->quest5->setText("I feel good about my performance");
            ui->quest6->setText("I am better able to cope\nwith my fear than before");
        }
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
    curRun.insert("prestress", ((double)ui->stress_slider->sliderPosition())/10);
    curRun.insert("poststress", ((double)ui->anxiety_slider->sliderPosition())/10);
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

    QVector<double> stressBefore, stressAfter, stressScores, indexes;
    QVector<QString> notes;

    int score = 0;

    QJsonArray runData;

    if(ui->scene_selection->currentIndex() == MainWindow::scene_idx_t::FEAR_OF_HEIGHTS)
    {
        runData = heightScene["runs"].toArray();
    }
    else if(ui->scene_selection->currentIndex() == MainWindow::scene_idx_t::SPEECH_ANXIETY)
    {
        runData = socialScene["runs"].toArray();
    }
    else
    {
        return;
    }

    QVector<double> attemptData, buildingHeights, ticks, successes;
    QVector<QString> labels;
    double maxHeight = 0;

    for(int iter = 0; iter < runData.size(); iter ++)
    {

        stressBefore.append(runData[iter].toObject()["prestress"].toDouble());
        stressAfter.append(runData[iter].toObject()["poststress"].toDouble());
        attemptData.append(runData[iter].toObject()["height"].toDouble());
        buildingHeights.append(runData[iter].toObject()["maxHeight"].toDouble()- runData[iter].toObject()["height"].toDouble());
        if(maxHeight < runData[iter].toObject()["maxHeight"].toDouble())
            maxHeight = runData[iter].toObject()["maxHeight"].toDouble();
        if(buildingHeights.at(iter) == 0)
            successes.append(runData[iter].toObject()["height"].toDouble() + 2);
        else
            successes.append(-50);
        score = runData[iter].toObject()["answers"].toObject()["1"].toInt() * -1 + 6;
        score += runData[iter].toObject()["answers"].toObject()["2"].toInt();
        score += runData[iter].toObject()["answers"].toObject()["3"].toInt();
        score += runData[iter].toObject()["answers"].toObject()["4"].toInt();
        score += runData[iter].toObject()["answers"].toObject()["5"].toInt();
        score += runData[iter].toObject()["answers"].toObject()["6"].toInt();
        score = (score / 36) * 10;
        stressScores.append(score);
        indexes.append((double)iter + 1);
        ticks.append(iter + 1);
        labels.append(QStringLiteral("Run %1").arg(iter + 1));
        score = 0;
        notes.append(runData[iter].toObject()["notes"].toString());
    }

    //---------------------------------------------------------------------------------------------------

    ui->customPlot->clearGraphs();
    ui->customPlot->clearPlottables();
    ui->graph2->clearGraphs();
    ui->graph2->clearPlottables();

    QLinearGradient gradient(0, 0, 0, 400);
    gradient.setColorAt(1, QColor(210,255,255));
    gradient.setColorAt(0, QColor(135,206,250));
    ui->graph2->setBackground(QBrush(gradient));
    ui->customPlot->setBackground(QBrush(gradient));

    ui->customPlot->legend->setVisible(true);
    ui->customPlot->legend->setFont(QFont("Helvetica", 9));

    ui->graph2->legend->setVisible(true);
    ui->graph2->legend->setFont(QFont("Helvetica", 9));

    QPen pen;
    QStringList lineNames;

    // add graphs with different line styles:

    ui->customPlot->addGraph();
    ui->customPlot->addGraph();
    ui->customPlot->addGraph();

    QCPBars *attempts = new QCPBars(ui->graph2->xAxis, ui->graph2->yAxis);
    attempts->setAntialiased(true);
    attempts->setStackingGap(0);
    attempts->setName("Height Reached");
    attempts->setPen(QPen(QColor(100, 100, 50).lighter(170)));
    attempts->setBrush(QColor(255, 200, 50));

    QCPBars *heights = new QCPBars(ui->graph2->xAxis, ui->graph2->yAxis);
    heights->setAntialiased(true);
    heights->setStackingGap(0);
    heights->setName("Remaining height To Top");
    heights->setPen(QPen(QColor(100, 100, 100).lighter(150)));
    heights->setBrush(QColor(105, 105, 105));

    heights->moveAbove(attempts);

    ui->graph2->yAxis->setRange(0, 15);
    ui->graph2->yAxis->setPadding(5); // a bit more space to the left border
    ui->graph2->yAxis->setLabel("Total Heights");

    QSharedPointer<QCPAxisTickerText> textTicker(new QCPAxisTickerText);
    textTicker->addTicks(ticks, labels);
    ui->graph2->xAxis->setTicker(textTicker);
    ui->graph2->xAxis->setSubTicks(false);
    ui->customPlot->xAxis->setTicker(textTicker);
    ui->customPlot->xAxis->setSubTicks(false);

    if(ticks.size() > 8)
    {
        ui->graph2->xAxis->setRange(0, 8.5);
        ui->customPlot->xAxis->setRange(0, 8.5);
    }

    //customPlot->xAxis->setBasePen(QPen(Qt::white));
    //customPlot->xAxis->setTickPen(QPen(Qt::white));
    //customPlot->xAxis->grid()->setVisible(true);
    //customPlot->xAxis->grid()->setPen(QPen(QColor(130, 130, 130), 0, Qt::DotLine));
    //customPlot->xAxis->setTickLabelColor(Qt::white);
    //customPlot->xAxis->setLabelColor(Qt::white);


    attempts->setData(ticks, attemptData);
    heights->setData(ticks, buildingHeights);


    ui->graph2->setInteractions(QCP::iRangeDrag | QCP::iRangeZoom);

    ui->graph2->addGraph();
    ui->graph2->graph(0)->setPen(QPen(QColor(240,0,0)));
    ui->graph2->graph(0)->setName("Made it to the top!");
    ui->graph2->graph(0)->setLineStyle((QCPGraph::LineStyle)0);
    ui->graph2->graph(0)->setScatterStyle(QCPScatterStyle(QPixmap(QApplication::applicationDirPath() + "/checkMark.png")));
    ui->graph2->graph(0)->setData(indexes,successes);




    ui->customPlot->graph(0)->setPen(QPen(QColor(240,0,0)));
    ui->customPlot->graph(0)->setName("General Stress Rating");
    ui->customPlot->graph(0)->setLineStyle((QCPGraph::LineStyle)1);
    ui->customPlot->graph(0)->setScatterStyle(QCPScatterStyle(QCPScatterStyle::ssDisc,8));
    ui->customPlot->graph(0)->setData(indexes,stressBefore);
    ui->customPlot->graph(1)->setPen(QPen(QColor(0,200,0)));
    ui->customPlot->graph(1)->setName("Post-Vr Stress Rating");
    ui->customPlot->graph(1)->setLineStyle((QCPGraph::LineStyle)1);
    ui->customPlot->graph(1)->setScatterStyle(QCPScatterStyle(QCPScatterStyle::ssDisc, 8));
    ui->customPlot->graph(1)->setData(indexes,stressAfter);
    ui->customPlot->graph(2)->setPen(QPen(QColor(0,0,200)));
    ui->customPlot->graph(2)->setName("General Survey Rating");
    ui->customPlot->graph(2)->setLineStyle((QCPGraph::LineStyle)1);
    ui->customPlot->graph(2)->setScatterStyle(QCPScatterStyle(QCPScatterStyle::ssDisc, 8));
    ui->customPlot->graph(2)->setData(indexes,stressScores);

    ui->customPlot->setInteractions(QCP::iRangeDrag | QCP::iRangeZoom);
    ui->customPlot->yAxis->setSubTicks(true);
    ui->customPlot->yAxis->setRange(-.5,10.5);


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
