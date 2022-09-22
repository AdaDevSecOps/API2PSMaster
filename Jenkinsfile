def githubRepo = 'https://github.com/AdaDevSecOps/API2PSMaster.git'
def githubBranch = 'main'

def dockerRepo = 'https://docker.io/jirayusamrit/adasoft.jenkin.api/'

pipeline
{
    agent any
    environment
    {
        dockerImage = ''
    }
    stages{
        stage("Git Clone")
        {
            steps
            {
                echo "========Cloning Git========"
                git url: githubRepo,
                    branch: githubBranch,
                    credentialsId:'adadev_creds'
            }
            post
            {
                success
                {
                    echo "========Cloning Git successfully========"
                }
                failure
                {
                    echo "========Cloning Git failed========"
                }
            }
        }

        stage('Docker Compose')
        {
            steps
            {
                echo "========Docker Building========"
                bat "docker-compose up -d --build"
            }
            post
            {
                success
                {
                    echo "========Docker Compose successfully========"
                }
                failure
                {
                    echo "========Docker Compose failed========"
                }
            }
        }
    }
}