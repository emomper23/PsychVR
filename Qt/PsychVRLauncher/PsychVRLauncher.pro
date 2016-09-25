#-------------------------------------------------
#
# Project created by QtCreator 2016-04-25T13:27:27
#
#-------------------------------------------------

QT       += core gui

greaterThan(QT_MAJOR_VERSION, 4): QT += widgets

TARGET = PsychVRLauncher
TEMPLATE = app


SOURCES += main.cpp\
        mainwindow.cpp \
    qglgraphwidget.cpp \
    cunitymap.cpp \
    cunityobject.cpp \
    editdialog.cpp \
    cuserstat.cpp

HEADERS  += mainwindow.h \
    qglgraphwidget.h \
    cunitymap.h \
    cunityobject.h \
    editdialog.h \
    cuserstat.h

FORMS    += mainwindow.ui \
    editdialog.ui
